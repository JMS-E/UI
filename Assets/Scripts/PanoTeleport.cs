//========= Copyright 2016-2019, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.Pointer3D;
using HTC.UnityPlugin.VRModuleManagement;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

#if VIU_STEAMVR_2_0_0_OR_NEWER
#endif

namespace HTC.UnityPlugin.Vive
{
    [AddComponentMenu("ETVR/PanoTeleport", 3)]
    public class PanoTeleport : MonoBehaviour, ReticlePoser.IMaterialChanger, IPointer3DPressExitHandler, IPointerEnterHandler, IPointerExitHandler

    {
        private Vector3 _panoramaPosition;
        private GameObject _panoramacontroler;
        private Coroutine teleportCoroutine;

        [InfoBox("fill this field with the panorama-skybox material")]
        [GUIColor(0.6f, 1f, 0.6f, 1f)]
        [FoldoutGroup("Panorama Setting")]
        public Material _panoramaName;

        [InfoBox("target: The actual transfrom that will be moved Ex. CameraRig, this will be autofilled on reset")]
        [GUIColor(0.6f, 1f, 0.6f, 1f)]
        [FoldoutGroup("teleporter Rig settings")]
        private Transform target;  

        [InfoBox("pivot: The actual pivot point that want to be teleported to the pointed location Ex. CameraHead, , this will be autofilled on reset")]
        [GUIColor(0.6f, 1f, 0.6f, 1f)]
        [FoldoutGroup("teleporter Rig settings")]
        private Transform pivot; 

        [FoldoutGroup("panorama teleporter settings")]
        public float fadeDuration = 0.3f;

        [SerializeField]
        [FoldoutGroup("panorama teleporter settings")]
        public TeleportButton teleportButton = TeleportButton.Trigger;

       [FoldoutGroup("panorama teleporter settings")]
        [SerializeField]
        private Material m_reticleMaterial;

        [FoldoutGroup("panorama teleporter settings")]
        [InfoBox("changes the color of the hoovering linerender reticle on the panoramaspot object")]
        public Material reticleMaterial { get { return m_reticleMaterial; } set { m_reticleMaterial = value; } }


        //highligther
        private MeshRenderer receivingRenderer;

        ////[InfoBox("changes the color of the panoramaspot Highlight")]
        ////[FoldoutGroup("panorama teleporter settings")]
        ////public Color colorDefault = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        ////[FoldoutGroup("panorama teleporter settings")]
        ////public Color colorHighlighted = new Color(0.0f, 1.0f, 0.0f, 1.0f);


        public enum TeleportButton
        {
            Trigger,
            Pad,
            Grip
        }

        // raycast enters collider
        public void OnPointerEnter(PointerEventData data)
        {
            //receivingRenderer.material.SetColor("_TintColor", colorHighlighted);
            receivingRenderer.enabled = true;


        }
        // raycast exits collider
        public void OnPointerExit(PointerEventData data)
        {
            //receivingRenderer.material.SetColor("_TintColor", colorDefault);
            receivingRenderer.enabled = false;
        }


        private void Start()
        {
            receivingRenderer = GetComponentInChildren<MeshRenderer>(); //reference to highligther
            receivingRenderer.enabled = false;

            _panoramacontroler = GameObject.FindGameObjectWithTag("PanoramaController");
            _panoramaPosition = GetComponent<Transform>().position;  //to teleport the player to the panoposition on exit
            //target = GameObject.Find("ViveRig").GetComponent<Transform>();
            if (XRDevice.isPresent)
            {
                target = GameObject.Find("ViveCameraRig").GetComponent<Transform>();
            }
            pivot = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        }

#if UNITY_EDITOR

        private void Reset()
        {
            FindTeleportPivotAndTarget();

            var scriptDir = System.IO.Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.MonoScript.FromMonoBehaviour(this)));
            if (!string.IsNullOrEmpty(scriptDir))
            {
                m_reticleMaterial = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(scriptDir.Replace("Scripts/Misc", "Materials/Reticle.mat"));
            }
            //fill in the links in the inspector
            //  target = GameObject.Find("ViveRig").GetComponent<Transform>();
            //  pivot = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        }

#endif

        private void FindTeleportPivotAndTarget()
        {
            foreach (var cam in Camera.allCameras)
            {
                if (!cam.enabled) { continue; }
#if UNITY_5_4_OR_NEWER
                // try find vr camera eye
                if (cam.stereoTargetEye != StereoTargetEyeMask.Both) { continue; }
#endif
                pivot = cam.transform;
                target = cam.transform.root == null ? cam.transform : cam.transform.root;
            }
        }

        public void OnPointer3DPressExit(Pointer3DEventData eventData)
        {
            // skip if it was teleporting
            if (teleportCoroutine != null) { return; }

            // skip if it was not releasing the button
            if (eventData.GetPress()) { return; }

            // check if is teleport button
            VivePointerEventData viveEventData;
            if (eventData.TryGetViveButtonEventData(out viveEventData))
            {
                switch (teleportButton)
                {
                    case TeleportButton.Trigger: if (viveEventData.viveButton != ControllerButton.Trigger) { return; } break;
                    case TeleportButton.Pad: if (viveEventData.viveButton != ControllerButton.Pad) { return; } break; //switched ControllerButton.DPadDown for ControllerButton.Pad
                    case TeleportButton.Grip: if (viveEventData.viveButton != ControllerButton.Grip) { return; } break;
                }
            }
            else if (eventData.button != (PointerEventData.InputButton)teleportButton)
            {
                switch (teleportButton)
                {
                    case TeleportButton.Trigger: if (eventData.button != PointerEventData.InputButton.Left) { return; } break;
                    case TeleportButton.Pad: if (eventData.button != PointerEventData.InputButton.Right) { return; } break;
                    case TeleportButton.Grip: if (eventData.button != PointerEventData.InputButton.Middle) { return; } break;
                }
            }

            var hitResult = eventData.pointerCurrentRaycast;


            //if (hitResult.gameObject.CompareTag("Teleportmarker"))
            //{
            //    Debug.Log("TELEPORTMARKER");
            //}


            // check if hit something
            if (!hitResult.isValid) { return; }

            if (target == null || pivot == null)
            {
                FindTeleportPivotAndTarget();
            }

            var headVector = Vector3.ProjectOnPlane(pivot.position - target.position, target.up);
            var targetPos = hitResult.worldPosition - headVector;

            if (VRModule.activeModule != VRModuleActiveEnum.SteamVR && fadeDuration != 0f)
            {
                Debug.LogWarning("Install SteamVR plugin and enable SteamVRModule support to enable fading");
                fadeDuration = 0f;
            }

            teleportCoroutine = StartCoroutine(StartTeleport(targetPos, fadeDuration));
        }

        private bool m_steamVRFadeInitialized;

        public IEnumerator StartTeleport(Vector3 position, float duration)
        {
#if VIU_STEAMVR && !VIU_STEAMVR_2_0_0_OR_NEWER
            var halfDuration = Mathf.Max(0f, duration * 0.5f);

            if (VRModule.activeModule == VRModuleActiveEnum.SteamVR && !Mathf.Approximately(halfDuration, 0f))
            {
                if (!m_steamVRFadeInitialized)
                {
                    // add SteamVR_Fade to the last rendered stereo camera
                    var fadeScripts = FindObjectsOfType<SteamVR_Fade>();
                    if (fadeScripts == null || fadeScripts.Length <= 0)
                    {
                        var topCam = SteamVR_Render.Top();
                        if (topCam != null)
                        {
                            topCam.gameObject.AddComponent<SteamVR_Fade>();
                        }
                    }
                    m_steamVRFadeInitialized = true;
                }

                SteamVR_Fade.Start(new Color(0f, 0f, 0f, 1f), halfDuration);
                yield return new WaitForSeconds(halfDuration);
                yield return new WaitForEndOfFrame(); // to avoid from rendering guideline in wrong position
                target.position = position;
                SteamVR_Fade.Start(new Color(0f, 0f, 0f, 0f), halfDuration);
                yield return new WaitForSeconds(halfDuration);
            }
            else
#endif
            {
                yield return new WaitForEndOfFrame(); // to avoid from rendering guideline in wrong position

                //ETVR show panorama
                _panoramacontroler.GetComponent<PanoramaControllerLogic>().ShowPanorama(_panoramaName);

                target.position = position;
            }
            teleportCoroutine = null;
        }
    }
}
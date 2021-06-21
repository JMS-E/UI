/// <summary>
///     2019 alexander.de.bruijn@politie.nl
///     
///     dependency
///     -sirenix odin inspector and dserializer
///     -htc vive input utillity
/// </summary>

using System.Collections;
using System.Collections.Generic;
using HTC.UnityPlugin.Vive;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.XR;

public class PanoramaControllerLogic : MonoBehaviour
{
    [Header("enable testmode, a-key prints player position")]
    [GUIColor(0.6f, 1f, 0.6f, 1f)]
    public bool __testmode = false;

    [BoxGroup("Pano Mode Visibillity Controlls")]
    [InfoBox("Fill this Array with all the objects that should be disabled when inside a panorama (eg: all visible meshes), all elements will be re-enabled when you exit the panoroma. ", InfoMessageType.None)]
    [GUIColor(0.6f, 1f, 0.6f, 1f)]
    public GameObject[] _objectsToDisable;

    int _enableStateCount;
    bool[] _enableStateStorage;

    [GUIColor(0.6f, 1f, 0.6f, 1f)]
    [FoldoutGroup("Zoom Attributes")]
    [Range(0, 180)]
    public int _defaultZoom = 60;

    [GUIColor(0.6f, 1f, 0.6f, 1f)]
    [FoldoutGroup("Zoom Attributes")]
    [Range(0, 180)]
    public int _magnifyZoom = 20;

    [GUIColor(0.6f, 1f, 0.6f, 1f)]
    [FoldoutGroup("Panorama Image Enhancement Attributes")]
    [Range(0f, 3f)]
    public float exposureSteps = 0.1f;

    private float currentexposure;
    private Material _defaultSkybox;
    private bool _panoramaMode = false;
    private GameObject _playerMaincamera;

    private GameObject controlerReference;

    public PointCloudController PointCloudController;
    public GameObject GeometrieContainer;

    private bool isShowingPointCloud = false;

    void Awake()
    {
        controlerReference = GameObject.FindGameObjectWithTag("Controller");
        _enableStateCount = _objectsToDisable.Length;
        _enableStateStorage = new bool[_enableStateCount];
        //set this objects TAG
        this.gameObject.tag = "PanoramaController";
        _playerMaincamera = GameObject.FindGameObjectWithTag("MainCamera");
        //store the default skybox texture
        _defaultSkybox = RenderSettings.skybox;
    }

    private void Start()
    {
        if (PointCloudController == null)
            PointCloudController = GameObject.Find("PanoramaController").GetComponent<PointCloudController>();
    }

    public void Update2()
    {

        if (_panoramaMode)
        {
            if (ViveInput.GetPressUpEx(HandRole.RightHand, ControllerButton.Trigger))
            {

                ExitPanorama();
            }
            if (Input.GetMouseButtonUp(0) || Input.GetKey(KeyCode.Mouse0))
            {
                ExitPanorama();
            }

            if (Input.GetKeyDown(KeyCode.Period) || (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.DPadRight)) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.DPadRight)))
            {
                DarkenPanorama();

            }
            if (Input.GetKeyDown(KeyCode.Comma) || (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.DPadLeft)) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.DPadLeft)))
            {
                LightenPanorama();
            }
            if (Input.GetKeyDown(KeyCode.R) || (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.DPadUp)) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.DPadUp)))
            {
                ResetPanoramaExposure();
            }
        }

        //Debug Codeblock
        if (__testmode)
        {
            //prints position of maincam
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log(_playerMaincamera.transform.position);
            }
        }
    }

    //ShowPanorama is called by the player activaded gameobject element
    public void ShowPanorama(Material panoramaMaterial)
    //public void ShowPanorama(Material panoramaMaterial, /*Vector3 panoramaPosition)
    {
        isShowingPointCloud = ! GeometrieContainer.activeSelf;

        _panoramaMode = true;
        RenderSettings.skybox = panoramaMaterial;
        DynamicGI.UpdateEnvironment();
        if (XRDevice.isPresent)
        {
            controlerReference.GetComponent<Controller>().toggleTablet();
        }

        if (isShowingPointCloud)
        {
            PointCloudController.HidePointCloud();
        }
        else
        {
            GeometrieContainer.SetActive(false);
        }


        //disable all other objects
        for (int ii = 0; ii < _objectsToDisable.Length; ii++)
        {
            //store current hierarchy state
            _enableStateStorage[ii] = _objectsToDisable[ii].activeSelf;
            _objectsToDisable[ii].SetActive(false);
        }


    }

    public void DarkenPanorama()
    {
        currentexposure = RenderSettings.skybox.GetFloat("_Exposure");
        RenderSettings.skybox.SetFloat("_Exposure", (currentexposure + exposureSteps));
        DynamicGI.UpdateEnvironment();
    }

    public void LightenPanorama()
    {
        currentexposure = RenderSettings.skybox.GetFloat("_Exposure");
        RenderSettings.skybox.SetFloat("_Exposure", (currentexposure - exposureSteps));
        DynamicGI.UpdateEnvironment();
    }

    private static void ResetPanoramaExposure()
    {
        RenderSettings.skybox.SetFloat("_Exposure", 1);
    }

    private void ZoomOut()
    {
        Camera.main.fieldOfView = _defaultZoom;
    }

    private void ZoomIn()
    {
        Camera.main.fieldOfView = _magnifyZoom;
    }

    public void ExitPanorama()
    {
        Debug.Log("***start exitpanorama");
        //switch skybox to default
        RenderSettings.skybox = _defaultSkybox;
        DynamicGI.UpdateEnvironment(); //debug?
        if (XRDevice.isPresent)
        {
            controlerReference.GetComponent<Controller>().toggleTablet();
        }

        //Reenable all disabled objects
        for (int ii = 0; ii < _enableStateStorage.Length; ii++)
        {
            //restore previous hierarchy state
            _objectsToDisable[ii].SetActive(_enableStateStorage[ii]);
        }

        if (isShowingPointCloud)
        {

            PointCloudController.ShowPointCloud();
        }
        else
        {
            GeometrieContainer.SetActive(true);
        }

        _panoramaMode = false;
    }
}
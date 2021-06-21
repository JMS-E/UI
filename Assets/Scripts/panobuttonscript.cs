/// <summary>
///     2020 alexander.de.bruijn@politie.nl
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using HTC.UnityPlugin.Vive;

using UnityEngine.XR;

public class panobuttonscript : MonoBehaviour
{
    private Vector3 _panoramaPosition;

    public GameObject _panoramacontroler;

    [InfoBox("fill this field with the panorama-skybox material")]
    [GUIColor(0.6f, 1f, 0.6f, 1f)]
    [FoldoutGroup("Panorama Setting")]
    public Material _panoramaName;

    [FoldoutGroup("needed for initialisation")]
    [Header("set camera to this heigth on exitting a panorama.")]
    public float PlayerLength = 1.75f;

    [FoldoutGroup("needed for initialisation")]
    [GUIColor(1f, 0.6f, 0.6f, 1f)]
    [Header("fill this field with ViveCameraRig")]
    public GameObject VRRig;

    [FoldoutGroup("needed for initialisation")]
    [GUIColor(1f, 0.6f, 0.6f, 1f)]    
    [Header("fill this field with the FirstPersonRig")]
    public GameObject FPCamera;
    
    void Awake()
    {
        //_panoramacontroler = GameObject.FindGameObjectWithTag("PanoramaController");
        _panoramaPosition = GetComponent<Transform>().position;  //to teleport the player to the panoposition on exit
        
        //target = GameObject.Find("ViveRig").GetComponent<Transform>();
        // if (XRDevice.isPresent)
        // {
        //     target = GameObject.Find("ViveCameraRig").GetComponent<Transform>();
        // }
        // pivot = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        }

    public void SwitchToPanorama()
    {
        if (ViveInput.GetPressUpEx(HandRole.RightHand, ControllerButton.Trigger))
        {   
            StartCoroutine(WaitForEventClear());
            //_panoramacontroler.GetComponent<PanoramaControllerLogic>().ShowPanorama(_panoramaName);
        }
        if (!XRDevice.isPresent)
        {
            StartCoroutine(WaitForEventClear());
            FPCamera.transform.position = new Vector3 (_panoramaPosition.x, PlayerLength, _panoramaPosition.z );
        }
    }
    
    IEnumerator WaitForEventClear()
    {

        yield return new WaitForSeconds(0.05f);
            _panoramacontroler.GetComponent<PanoramaControllerLogic>().ShowPanorama(_panoramaName);
    }
}

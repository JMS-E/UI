using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;


public class panoramaButon : MonoBehaviour
{
    [Header("return height after exiting a panoramapicture")]
    public float Playerlength = 1.75f;

    [Header("fill this field with the desired panorama material")]
    [FormerlySerializedAs("PanoramaName")] 
    public Material _panoramaName;

    [Header("fill this field with ViveCameraRig")]
    public GameObject vrRig;
    
    [Header("fill this field with the FirstPersonRig")]
    public GameObject FPCamera;
    
    private Vector3 _panoramaPosition;
    
    private GameObject _panoramacontroler;


    private void Start()
    {
        _panoramacontroler = GameObject.FindGameObjectWithTag("PanoramaController");
        _panoramaPosition = GetComponent<Transform>().position;  //to teleport the player to the panoposition on exit
    }

    void OnMouseUp()
    {
        Debug.Log("***MOUSE");
        EnterPanoMode();
    }

    public void EnterPanoMode()
    {
        Debug.Log("*** enter pano");

        _panoramacontroler.GetComponent<PanoramaControllerLogic>().ShowPanorama(_panoramaName);
        if (!XRDevice.isPresent)
        {
            FPCamera.transform.position = new Vector3 (_panoramaPosition.x, Playerlength, _panoramaPosition.z );

        }
    }
}
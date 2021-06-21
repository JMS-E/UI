/// <summary>
/// alexDB 2019
/// ETVR template generic controller asset
/// Controller component
///
/// </summary>

using System.Collections;
using System.Collections.Generic;
using HTC.UnityPlugin.Vive;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class Controller : MonoBehaviour
{
    //pointcloud toggle
    public GameObject _PointcloudHolder;
    public GameObject _geo;
    public Teleportable teleportable;
    public bool TeleportOnAtStart;

    private PointCloudController pcc;

    //xr device detection
    [Header("references to the vr an dfp controller rigs")]
    public GameObject vrRig; //a reference to thr vr rig
    public GameObject fpRig; // a reference to the first person camera controller


    //day night toggle
    [Header("daynight switch preferences")]
    public float brightnesCorrection = 1;
    public Color nightColor = new Color(0.15f, 0.15f, 0.15f);
    public Color dayColor = new Color(0.95f, 0.95f, 0.95f);
    private GameObject _MainLight;
    private bool _isdaystate = true;

    //controllertablet
    private GameObject _ControllTablet;
    private GameObject _Magnifier;
    private GameObject _flashlight;

    //panoramacontroller    
    public GameObject _panoramacontroler;



    void Awake()
    {

        //HMD detection
        if (XRDevice.isPresent)
        {
            Debug.Log("***xrdevice is present");
            vrRig.SetActive(true);
            fpRig.SetActive(false);
        }
        else 
        {
            Debug.Log("***no xr device");
            vrRig.SetActive(false);
            fpRig.SetActive(true);
        }

        //auto find references based on tags
        _MainLight = GameObject.FindGameObjectWithTag("MainLight");
        //_PointcloudHolder = GameObject.FindGameObjectWithTag("pointcloud");
        _geo = GameObject.FindGameObjectWithTag("Geometry");
        _ControllTablet = GameObject.FindGameObjectWithTag("ControllTablet");
        _Magnifier = GameObject.FindGameObjectWithTag("Magnifier");
        //_flashlight = GameObject.FindGameObjectWithTag("Flashlight");

        //wait with disableing gameobjects until AFTER a reference is set or it will not be found during Awake()
        if (XRDevice.isPresent)
        {
            _Magnifier.SetActive(!_Magnifier.activeSelf);
        }
        // _flashlight.SetActive(!_flashlight.activeSelf);

    }

    void Start()
    {
        nightColor = new Color(nightColor.r * brightnesCorrection, nightColor.g * brightnesCorrection, nightColor.b * brightnesCorrection);
        dayColor = new Color(dayColor.r * brightnesCorrection, dayColor.g * brightnesCorrection, dayColor.b * brightnesCorrection);
        //Aanpassing
        //_PointcloudHolder.SetActive(false);
        teleportable.enabled = TeleportOnAtStart;
        //Aanpassing
        pcc = _PointcloudHolder.GetComponentInChildren<PointCloudController>();
    }

    // Update is called once per frame
    //implement button onmouseUP
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P)) //|| (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.DPadUp)))
        {
            //Aanpassing
            pcc.TogglePointCloud();
        }

        if (Input.GetKeyUp(KeyCode.L)) //TODO implement pannel buton
        {
            toggledaynight();
        }

        //if (Input.GetKeyUp(KeyCode.O) ) //|| (ViveInput.GetPressUpEx(HandRole.RightHand, ControllerButton.Grip)))
        //{
        //    toggleFlashlight();
        //}

        if (Input.GetKeyUp(KeyCode.F5))
        {   
            ResetScene();
        }

        if (Input.GetKey("escape"))
        {   
            Debug.Log("escape pressed");
            Application.Quit();
            Debug.Log("after escape");
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            teleportable.enabled = (!teleportable.enabled);
        }

         _panoramacontroler.GetComponent<PanoramaControllerLogic>().Update2();
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void togglePointcloud()
    {
        _PointcloudHolder.SetActive(!_PointcloudHolder.activeSelf);
        _geo.SetActive(!_geo.activeSelf);

    }

    public void toggleTablet()
    {
        if (XRDevice.isPresent)
        {
            _ControllTablet.SetActive(!_ControllTablet.activeSelf);
            _Magnifier.SetActive(!_Magnifier.activeSelf);
        }    
    }


    //private void toggleFlashlight()
    //{
    //    _flashlight.SetActive(!_flashlight.activeSelf);

    //}

    public void toggledaynight()
    {
        if (_isdaystate == true)
        {

            _isdaystate = false;
            _MainLight.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            _MainLight.GetComponent<Light>().intensity = 0;
            RenderSettings.ambientLight = nightColor;

        }
        else
        {

            _isdaystate = true;
            _MainLight.transform.localRotation = Quaternion.Euler(90, 0, 0);
            _MainLight.GetComponent<Light>().intensity = 1;
            RenderSettings.ambientLight = dayColor;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class posetrackeroncollider : MonoBehaviour
{
    VivePoseTracker posetracker;
    void Start()
    {
        posetracker = GetComponent<VivePoseTracker>();
        posetracker.enabled = false;
    }


    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Grip))
        {
            posetracker.enabled = true;
        }

        if (ViveInput.GetPressUpEx(HandRole.RightHand, ControllerButton.Grip))
        {
            posetracker.enabled = false;
        }
    }
}

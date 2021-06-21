using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class hapticHighlight : MonoBehaviour
{

    void SinglePulse(ushort durationMicroSec)
    {
        ViveInput.TriggerHapticPulseEx<HandRole>(HandRole.RightHand, durationMicroSec);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SinglePulse(500);
        }

    }


    //void DoublePulse()
    //{
    //    ViveInput.TriggerHapticVibrationEx<HandRole>(HandRole.RightHand, float durationSeconds = 0.2f, float frequency = 85f, float amplitude = 0.125f, float startSecondsFromNow = 0f);
    //   ViveInput.TriggerHapticVibrationEx<HandRole>(HandRole.RightHand, float durationSeconds = 0.2f, float frequency = 85f, float amplitude = 0.125f, float startSecondsFromNow = 0.2f);
    //}
}

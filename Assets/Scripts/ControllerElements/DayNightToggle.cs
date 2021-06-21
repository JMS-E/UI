using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightToggle : MonoBehaviour
{
    private bool isdaystate = true;
    public bool LightInducedSeizureMode = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (LightInducedSeizureMode)
        {
            toggledaynight();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            toggledaynight();
        }

    }
    public void toggledaynight()
    {
        if (isdaystate == true)
        {

            isdaystate = !isdaystate;
            transform.localRotation = Quaternion.Euler(-90, 0, 0);
        }
        else
        {

            isdaystate = !isdaystate;
            transform.localRotation = Quaternion.Euler(90, 0, 0);
        }
    }
}
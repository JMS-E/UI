using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class followCam : MonoBehaviour
{
    public Camera m_cam;

    // Update is called once per frame
    void Update()
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        rectTransform.transform.eulerAngles = m_cam.transform.eulerAngles;
    }
}

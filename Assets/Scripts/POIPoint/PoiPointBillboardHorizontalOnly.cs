using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class PoiPointBillboardHorizontalOnly : MonoBehaviour
{

    [FoldoutGroup("Prefab setup attributes")]
    [InfoBox("do not change unless you have a good reason, you risk breaking this prefab, this reference will be set automatically based on the MainCamera tag")]
    [GUIColor(1f, 0.6f, 0.6f, 1f)]
    [Header("this holds the reference to the main camera.")]
    public Camera cameraToLookAt;

    private void Start()
    {
        cameraToLookAt = Camera.main;
    }

    void FixedUpdate()
    {
        Vector3 v = cameraToLookAt.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(cameraToLookAt.transform.position - v);
        transform.Rotate(0, 180, 0);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoiPointBillboarding : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
        transform.Rotate(0, 180, 0);
    }
}


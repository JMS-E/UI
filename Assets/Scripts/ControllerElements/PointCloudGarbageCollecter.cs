using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCloudGarbageCollecter : MonoBehaviour 
{
    public string objectName;
    public GameObject parentObject;
    
    public void GarbageCollector()
    {
        GameObject[] gos = (GameObject[])FindObjectsOfType(typeof(GameObject));
        var objectPath = $"{objectName}";

        for (int ii = 0; ii < gos.Length; ii++)
        {
            if (gos[ii].name.Contains(objectPath))
            {
                gos[ii].transform.parent = parentObject.transform;
            }
        }
    }

    private void Update()
    {
        GarbageCollector();
    }
}

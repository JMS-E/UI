using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass_rotation : MonoBehaviour
{
    public Transform player;
    Vector3 vector; 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        vector.z = player.eulerAngles.y;
        transform.localEulerAngles = vector;

    }
}

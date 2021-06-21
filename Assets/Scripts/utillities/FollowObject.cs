using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    //public inspector attributes
    [EnumPaging]
    public updateLoop chooseUpdateLoop;

    [Header("set the object to follow")]
    [Required]
    public GameObject target;

    [Header("en/dis -able rotation or position following")]
    public bool followTranslation;
    public bool followRotation;

    [FoldoutGroup("Translation Settings")]
    [Header("select wich position axis to follow")]
    public bool PositionX;

    [FoldoutGroup("Translation Settings")]
    public bool PositionY;

    [FoldoutGroup("Translation Settings")]
    public bool PositionZ;

    [FoldoutGroup("Translation Settings")]
    [Header("position ofset values")]
    public Vector3 PositionOfset;

    [FoldoutGroup("Rotation Settings")]
    [Header("Select wich rotation axis to follow")]
    public bool RotationX;

    [FoldoutGroup("Rotation Settings")]
    public bool RotationY;

    [FoldoutGroup("Rotation Settings")]
    public bool RotationZ;

    [FoldoutGroup("Rotation Settings")]
    [Header("Rotation ofset values")]
    public Vector3 rotationOfset;

    //private variables
    private Vector3 _followPositionCoordinates;
    private Vector3 _followRotationCoordinates;
    private Vector3Int _followTargetPosition;
    private Vector3Int _followTargetPositionInverse;
    private Vector3Int _followTargetRotation;
    private Vector3Int _followTargetRotationInverse;

    //updateloop enum variables
    private bool _onUpdate;
    private bool _onFixedUpdate;
    private bool _onLateUpdate;

    public enum updateLoop
    {
        Update, FixedUpdate, LateUpdate
    }
    private void Awake()
    {
        //when the follower should move the object
        switch (chooseUpdateLoop)
        {
            case updateLoop.Update:
                _onUpdate = true;
                break;
            case updateLoop.FixedUpdate:
                _onFixedUpdate = true;
                break;
            case updateLoop.LateUpdate:
                _onLateUpdate = true;
                break;
            default:
                break;
        }
    }

    void Start()
    {
        //process inputsettings from the inspector to an array
        _followTargetPosition = new Vector3Int(Convert.ToInt32(PositionX), Convert.ToInt32(PositionY), Convert.ToInt32(PositionZ));
        _followTargetPositionInverse = new Vector3Int(Convert.ToInt32(!PositionX), Convert.ToInt32(!PositionY), Convert.ToInt32(!PositionZ));

        _followTargetRotation = new Vector3Int(Convert.ToInt32(RotationX), Convert.ToInt32(RotationY), Convert.ToInt32(RotationZ));
        _followTargetRotationInverse = new Vector3Int(Convert.ToInt32(!RotationX), Convert.ToInt32(!RotationY), Convert.ToInt32(!RotationZ));
    }

    void Update()
    {
        if (_onUpdate)
        {
            followObject();
        }
    }

    void LateUpdate()
    {
        if (_onLateUpdate)
        {
            followObject();
        }
    }

    private void FixedUpdate()
    {
        if (_onFixedUpdate)
        {
            followObject();
        }
    }


    private Vector3 TranslateObject()
    {
        //depending on settings, coordinates are multiplied by 0 or 1 to follow either the target or stay at their position.
        _followPositionCoordinates.x = (target.transform.position.x * _followTargetPosition.x) + (gameObject.transform.position.x * _followTargetPositionInverse.x);
        _followPositionCoordinates.y = (target.transform.position.y * _followTargetPosition.y) + (gameObject.transform.position.y * _followTargetPositionInverse.y);
        _followPositionCoordinates.z = (target.transform.position.z * _followTargetPosition.z) + (gameObject.transform.position.z * _followTargetPositionInverse.z);
        return _followPositionCoordinates + PositionOfset;
    }

    private Vector3 RotateObject()
    {
        _followRotationCoordinates.x = ((target.transform.rotation.eulerAngles.x * _followTargetRotation.x) + (gameObject.transform.rotation.eulerAngles.x * _followTargetRotationInverse.x));
        _followRotationCoordinates.y = ((target.transform.rotation.eulerAngles.y * _followTargetRotation.y) + (gameObject.transform.rotation.eulerAngles.y * _followTargetRotationInverse.y));
        _followRotationCoordinates.z = ((target.transform.rotation.eulerAngles.z * _followTargetRotation.z) + (gameObject.transform.rotation.eulerAngles.z * _followTargetRotationInverse.z));
        return _followRotationCoordinates + rotationOfset;
    }

    private void followObject()
    {
        //turn rotation and or translation on or off.
        if (followTranslation)
        {
            gameObject.transform.position = TranslateObject();
        }
        if (followRotation)
        {
            gameObject.transform.rotation = Quaternion.Euler(RotateObject());
        }
    }
}

/************************************************************
*************2020 alexander.de.bruijn@politie.nl*************
*************************************************************/
/// <summary>
/// assign to let an object follow a defined target, rotation and position can be enabled per axis
/// TODO: inverse axis
/// TODO: remap axis, eg: objext1.position.X -> object2.position.Y
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class FollowObject1 : MonoBehaviour
{
    //public inspector attributes
    [EnumPaging]
    public updateLoop chooseUpdateLoop;

    [Header("Choose the object to follow")]
    [Required]
    public GameObject target;

    [Header("En/Dis -able rotation or position following")]
    public bool followTranslation;
    public bool followRotation;

    [FoldoutGroup("Translation Settings")]
    [Header("Choose wich position axis to follow")]
    public bool positionX;

    [FoldoutGroup("Translation Settings")]
    public bool positionY;

    [FoldoutGroup("Translation Settings")]
    public bool positionZ;

    [FoldoutGroup("Translation Settings")]
    [Header("Position ofset values")]
    public Vector3 positionOfset;

    [FoldoutGroup("Rotation Settings")]
    [Header("Choose wich rotation axis to follow")]
    public bool rotationX;

    [FoldoutGroup("Rotation Settings")]
    public bool rotationY;

    [FoldoutGroup("Rotation Settings")]
    public bool rotationZ;

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
        //process inputsettings from the inspector so we can multiply the rotation and translation values by 1 or 0 depending on if its axis is (en/dis)abled 
        _followTargetPosition = new Vector3Int(Convert.ToInt32(positionX), Convert.ToInt32(positionY), Convert.ToInt32(positionZ));
        _followTargetPositionInverse = new Vector3Int(Convert.ToInt32(!positionX), Convert.ToInt32(!positionY), Convert.ToInt32(!positionZ));

        //inverse array is used to avoid complex if-else functions in the translateObject en rotateObject methods
        _followTargetRotation = new Vector3Int(Convert.ToInt32(rotationX), Convert.ToInt32(rotationY), Convert.ToInt32(rotationZ));
        _followTargetRotationInverse = new Vector3Int(Convert.ToInt32(!rotationX), Convert.ToInt32(!rotationY), Convert.ToInt32(!rotationZ));
    }

    void Update()
    {
        if (_onUpdate) followObject(followTranslation, followRotation);
    }

    void LateUpdate()
    {
        if (_onLateUpdate) followObject(followTranslation, followRotation);
    }

    void FixedUpdate()
    {
        if (_onFixedUpdate) followObject(followTranslation, followRotation);
    }


    private Vector3 TranslateObject()
    {
        //depending on settings, coordinates are multiplied by 0 or 1 to follow either the target or stay at their position.
        _followPositionCoordinates.x =  (positionOfset.x + (target.transform.position.x * _followTargetPosition.x)) + 
                                        (gameObject.transform.position.x * _followTargetPositionInverse.x);
        _followPositionCoordinates.y =  (positionOfset.y + (target.transform.position.y * _followTargetPosition.y))+ 
                                        (gameObject.transform.position.y * _followTargetPositionInverse.y);
        _followPositionCoordinates.z =  (positionOfset.z + (target.transform.position.z * _followTargetPosition.z)) + 
                                        (gameObject.transform.position.z * _followTargetPositionInverse.z);
        return _followPositionCoordinates;
    }

    private Vector3 RotateObject()
    {
        _followRotationCoordinates.x =  (rotationOfset.x + (target.transform.rotation.eulerAngles.x * _followTargetRotation.x)) +
                                        (gameObject.transform.rotation.eulerAngles.x * _followTargetRotationInverse.x);
        _followRotationCoordinates.y =  (rotationOfset.y + (target.transform.rotation.eulerAngles.y * _followTargetRotation.y)) +
                                        (gameObject.transform.rotation.eulerAngles.y * _followTargetRotationInverse.y);
        _followRotationCoordinates.z =  (rotationOfset.z + (target.transform.rotation.eulerAngles.z * _followTargetRotation.z)) +
                                        (gameObject.transform.rotation.eulerAngles.z * _followTargetRotationInverse.z);

        return _followRotationCoordinates;
    }

    private void followObject(bool translationISEnabled, bool rotationIsEnabled)
    {
        //turn rotation and or translation on or off.
        if (translationISEnabled)
        {
            gameObject.transform.position = TranslateObject();
        }
        if (rotationIsEnabled)
        {
            gameObject.transform.rotation = Quaternion.Euler(RotateObject());
        }
    }
}

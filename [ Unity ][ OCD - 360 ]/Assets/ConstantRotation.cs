using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    #region [ Serialised Fields ]
    [SerializeField] private Vector3 _rotation;
    #endregion

    #region [ Unserialised Fields ]
    private Transform _transform;
    #endregion



    private void Awake() => _transform = transform;


    private void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Euler(_rotation);
        _transform.rotation = rotation * _transform.rotation;
    }
}

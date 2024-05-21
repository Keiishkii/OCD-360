using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReminderUI : MonoBehaviour
{
    #region [ Serialised Field ]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private GameObject _reminderUIGameObject;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _rotationOffset;
    [SerializeField] private float _lerp;
    #endregion

    #region [ Unserialised Fields ]
    private Transform _transform;
    private Vector3 _currentForwardDirection = Vector3.forward;
    private bool _visible;
    public bool Visible
    {
        get => _visible;
        set
        {
            _visible = value;
            _reminderUIGameObject.SetActive(value);
        }
    }
    #endregion



    private void Awake() => _transform = _reminderUIGameObject.transform;
    private void Start()
    {
        _transform.position = _offset;
        
        Quaternion rotation = Quaternion.identity;
        Vector3 position = rotation * _offset;
        
        _transform.SetPositionAndRotation(position, rotation);
        Visible = false;
    }

    private void FixedUpdate()
    {
        Vector3 targetForward = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up);
        _currentForwardDirection = Vector3.Normalize(Vector3.Slerp(_currentForwardDirection, targetForward, _lerp));

        Quaternion lookRotation = Quaternion.LookRotation(_currentForwardDirection);

        Quaternion rotation = Quaternion.Euler(_rotationOffset) * lookRotation;
        Vector3 position = lookRotation * _offset;
        
        _transform.SetPositionAndRotation(position, rotation);
    }
}

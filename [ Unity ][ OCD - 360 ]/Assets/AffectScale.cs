using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AffectScale : MonoBehaviour
{
    #region [ Instance ]
    private static AffectScale _instance;
    public static AffectScale Instance => _instance ? _instance : _instance = FindObjectOfType<AffectScale>();
    #endregion
    
    #region [ Serialised Fields ]
    [SerializeField] private InputActionReference _rightJoystickInputActionReference;
    [SerializeField] private GameObject _cursor;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _offsetMultiplier;
    #endregion
    
    #region [ Unserialised Fields ]
    private RectTransform _cursorTransform;
    private RectTransform _targetTransform;

    public Vector2 Value { get; private set; }
    #endregion

    
    
    

    private void Awake()
    {
        _cursorTransform = _cursor.transform as RectTransform;
        _targetTransform = _target.transform as RectTransform;
    }

    private void OnEnable()
    {
        _rightJoystickInputActionReference.action.performed += OnRightJoystickInputPerformed;
        _rightJoystickInputActionReference.action.canceled += OnRightJoystickInputCancelled;
        
        SetCursorPosition(Vector2.zero);
    }
    
    private void OnDisable()
    {
        _rightJoystickInputActionReference.action.performed -= OnRightJoystickInputPerformed;
        _rightJoystickInputActionReference.action.canceled -= OnRightJoystickInputCancelled;
    }
    
    
    
    private void SetCursorPosition(Vector2 position) => _cursorTransform.localPosition = _offsetMultiplier * position;
    public void SetTargetPosition(Vector2 position) => _targetTransform.localPosition = _offsetMultiplier * position;
    public void SetTargetVisible(bool visibility) => _target.SetActive(visibility);
    
    
    
    private void OnRightJoystickInputPerformed(InputAction.CallbackContext context)
    {
        Value = context.ReadValue<Vector2>();
        SetCursorPosition(Value);
        
        Debug.Log($"Performed: {Value}");
    }
    
    private void OnRightJoystickInputCancelled(InputAction.CallbackContext context)
    {
        Value = Vector2.zero;
        SetCursorPosition(Vector2.zero);
        Debug.Log($"Cancelled: {Value}");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using MINELab;
using UnityEngine;
using UnityEngine.InputSystem;

public class AffectScale : MonoBehaviour
{
    #region [ Instance ]
    private static AffectScale _instance;
    public static AffectScale Instance => _instance ? _instance : _instance = FindObjectOfType<AffectScale>();
    #endregion
    
    #region [ Serialised Fields ]
    [SerializeField] private GameObject _canvasGameObject;
    [SerializeField] private GameObject _cursor;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _offsetMultiplier;
    [SerializeField] private float _gizmosSize;
    #endregion
    
    #region [ Unserialised Fields ]
    private RectTransform _cursorTransform;
    private RectTransform _targetTransform;

    public bool Visible
    {
        get => _visible;
        set
        {
            _canvasGameObject.SetActive(value);
            _visible = value;
        }
    }
    private bool _visible;
    #endregion

    
    
    

    private void Awake()
    {
        _cursorTransform = _cursor.transform as RectTransform;
        _targetTransform = _target.transform as RectTransform;
    }

    private void OnEnable()
    {
        InputManager.Instance.rightJoystickValueChangedEvent.AddListener(SetCursorPosition);
        SetCursorPosition(Vector2.zero);
    }
    
    private void OnDisable()
    {
        if (InputManager.Instance is not { }) return;
        InputManager.Instance.rightJoystickValueChangedEvent.RemoveListener(SetCursorPosition);
    }

    
    
    private void SetCursorPosition(Vector2 position)
    {
        _cursorTransform.localPosition = _offsetMultiplier * position;
        Debug.Log($"Position: {position}");
    }

    public void SetTargetPosition(Vector2 position) => _targetTransform.localPosition = _offsetMultiplier * position;
    public void SetTargetVisible(bool visibility) => _target.SetActive(visibility);

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_targetTransform.position, _gizmosSize);
    }
}

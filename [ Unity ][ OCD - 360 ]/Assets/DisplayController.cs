using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayController : MonoBehaviour
{
    #region [ Serialised Fields ]
    [SerializeField] private GameObject _meshGameObject;
    [SerializeField] private MeshRenderer _triggerMeshRenderer;
    [SerializeField] private MeshRenderer _touchPadMeshRenderer;
    [SerializeField] private Material _controllerDefaultMaterial;
    [SerializeField] private Material _controllerHighlightMaterial;
    #endregion

    #region [ Unserialised Fields ]
    public bool Visible
    {
        get => _visible;
        set
        {
            _meshGameObject.SetActive(value);
            _visible = value;
        }
    }
    private bool _visible;
    
    private Material _runtimeControllerDefaultMaterial;
    private Material _runtimeControllerHighlightMaterial;
    #endregion


    
    private void Awake()
    {
        _runtimeControllerDefaultMaterial = new Material(_controllerDefaultMaterial);
        _runtimeControllerHighlightMaterial = new Material(_controllerHighlightMaterial);

        _triggerMeshRenderer.material = _runtimeControllerDefaultMaterial;
        _touchPadMeshRenderer.material = _runtimeControllerDefaultMaterial;
    }
    
    
    
    public void HighlightTrigger()
    {
        _triggerMeshRenderer.material = _runtimeControllerHighlightMaterial;
        _touchPadMeshRenderer.material = _runtimeControllerDefaultMaterial;
    }
    
    public void HighlightTouchPad()
    {
        _triggerMeshRenderer.material = _runtimeControllerDefaultMaterial;
        _touchPadMeshRenderer.material = _runtimeControllerHighlightMaterial;
    }
    
    public void ResetHighlight()
    {
        _triggerMeshRenderer.material = _runtimeControllerDefaultMaterial;
        _touchPadMeshRenderer.material = _runtimeControllerDefaultMaterial;
    }
}

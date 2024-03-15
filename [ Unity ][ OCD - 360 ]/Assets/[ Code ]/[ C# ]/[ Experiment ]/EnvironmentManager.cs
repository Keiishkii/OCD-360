using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    #region [ Instance ]
    private static EnvironmentManager _instance;
    public static EnvironmentManager Instance => _instance ? _instance : _instance = FindObjectOfType<EnvironmentManager>();
    #endregion
    
    #region [ Serialised Fields ]
    [SerializeField] private List<GameObject> _experimentRoom;
    
    [SerializeField] private GameObject _360EnvironmentSphere;
    [SerializeField] private GameObject _360EnvironmentSphereMesh;
    [SerializeField] private Transform _cameraTransform;
    #endregion

    #region [ Unserialised Fields ]
    private MeshRenderer _360SphereMeshRenderer;
    private Transform _360SphereTransform;
    #endregion



    private void Awake()
    {
        _360SphereTransform = _360EnvironmentSphere.transform;
        _360SphereMeshRenderer = _360EnvironmentSphereMesh.GetComponent<MeshRenderer>();
    }

    public void SetEnvironment(EnvironmentDataScriptableObject environmentData, float yRotationOffset)
    {
        foreach (GameObject gameObject in _experimentRoom) gameObject.SetActive(false);
        _360EnvironmentSphere.SetActive(true);

        Quaternion lookRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(_cameraTransform.transform.forward, Vector3.up));
        
        _360SphereTransform.rotation = Quaternion.Euler(0, yRotationOffset, 0) * lookRotation * Quaternion.Euler(environmentData.rotationOffset);
        _360SphereMeshRenderer.material = environmentData.material;
    }

    public void Return()
    {
        foreach (GameObject gameObject in _experimentRoom) gameObject.SetActive(true);
        _360EnvironmentSphere.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
using KeiishkiiLib;
using static KeiishkiiLib.InspectorUtility;
#endif

public class RoomController : MonoBehaviour
{
    #if UNITY_EDITOR
    #region [ Editor ]
    [CustomEditor(typeof(RoomController))]
    private class RoomEditor : CustomInspector<RoomController>
    {
        protected override void OnInspectorRender()
        {
            for (int i = 0; i < _targetScript._roomData.Count; i++)
            {
                Button($"Set Room: {i}", false, () => 
                {
                    _targetScript.SetRoomMaterial(i);
                });
            }
        }
    }
    #endregion
    #endif

    #region [ Serialised Fields ]
    [SerializeField] private Transform _transformTransform;
    [SerializeField] private MeshRenderer _roomMeshRenderer;
    [SerializeField] private List<RoomData> _roomData = new List<RoomData>();
    #endregion



    private void SetRoomMaterial(in int i)
    {
        _roomMeshRenderer.material = new Material(_roomData[i].material);
        _transformTransform.rotation = Quaternion.Euler(_roomData[i].eulerOffset);
    }

    private void OnDrawGizmos()
    {
        Transform transform = this.transform;
        Gizmos.DrawLine(transform.position + new Vector3(500, 0, 0), transform.position - new Vector3(500, 0, 0));
        Gizmos.DrawLine(transform.position + new Vector3(0, 500, 0), transform.position - new Vector3(0, 500, 0));
        Gizmos.DrawLine(transform.position + new Vector3(0, 0, 500), transform.position - new Vector3(0, 0, 5));
    }
}

[Serializable]
public struct RoomData
{
    public Material material;
    public Vector3 eulerOffset;
}
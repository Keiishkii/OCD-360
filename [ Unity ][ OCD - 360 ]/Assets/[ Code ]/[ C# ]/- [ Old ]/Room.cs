using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using KeiishkiiLib;
using static KeiishkiiLib.InspectorUtility;
using System;

public class RoomController : MonoBehaviour
{
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
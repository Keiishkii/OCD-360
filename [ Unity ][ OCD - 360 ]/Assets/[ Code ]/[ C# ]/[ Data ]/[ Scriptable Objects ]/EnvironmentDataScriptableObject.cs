using UnityEngine;

[CreateAssetMenu(fileName = "Environment Data", menuName = "[ MINE Lab ]/Environment Data")]
public class EnvironmentDataScriptableObject : ScriptableObject
{
    #region [ Serialised Fields ]
    public Material material;
    public Vector3 rotationOffset;
    #endregion
}

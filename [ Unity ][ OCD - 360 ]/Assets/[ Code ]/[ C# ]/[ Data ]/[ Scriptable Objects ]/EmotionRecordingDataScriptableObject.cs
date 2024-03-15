using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Emotion Data Recording UI Setup", menuName = "[ MINE Lab ]/Emotion Data Recording UI Setup Data")]
public class EmotionRecordingDataScriptableObject : ScriptableObject
{
    #region [ Serialised Fields ]
    [FormerlySerializedAs("emotionTexture")] public Sprite expressionImageSprite;
    [TextArea] public string instructions;
    public string displayName;
    #endregion
}

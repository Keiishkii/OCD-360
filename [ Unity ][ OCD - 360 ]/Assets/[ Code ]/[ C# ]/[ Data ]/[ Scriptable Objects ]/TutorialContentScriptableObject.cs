using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Tutorial Content", menuName = "[ MINE Lab ]/Tutorial Content Data")]
public class TutorialContentScriptableObject : ScriptableObject
{
    #if UNITY_EDITOR
    #region [ Editor ]
    [CustomEditor(typeof(TutorialContentScriptableObject))]
    public class TutorialContentScriptableObjectEditor : KeiishkiiLib.CustomInspector<TutorialContentScriptableObject>
    {
        protected override void OnInspectorRender()
        {
            EditorStyles.textField.wordWrap = true;
            KeiishkiiLib.InspectorUtility.BooleanField("Display Title:", ref _targetScript.displayTitle);
            if (_targetScript.displayTitle) _targetScript.title = EditorGUILayout.TextArea(_targetScript.title);
            
            KeiishkiiLib.InspectorUtility.Separator();
            KeiishkiiLib.InspectorUtility.BooleanField("Display Content:", ref _targetScript.displayContent);
            if (_targetScript.displayContent) _targetScript.content = EditorGUILayout.TextArea(_targetScript.content);
            
            KeiishkiiLib.InspectorUtility.Separator();
            KeiishkiiLib.InspectorUtility.BooleanField("Display Continue Condition:", ref _targetScript.displayContinueCondition);
            if (_targetScript.displayContinueCondition) _targetScript.continueCondition = EditorGUILayout.TextArea(_targetScript.continueCondition);
            
            EditorUtility.SetDirty(_targetScript);
        }
    }
    

    #endregion
    #endif
    
    #region [ Serialised Fields ]
    public bool displayTitle;
    [TextArea(1,10000)] public string title;
    public bool displayContent;
    [TextArea(8,10000)] public string content;
    public bool displayContinueCondition;
    [TextArea(1,10000)] public string continueCondition;
    #endregion
}

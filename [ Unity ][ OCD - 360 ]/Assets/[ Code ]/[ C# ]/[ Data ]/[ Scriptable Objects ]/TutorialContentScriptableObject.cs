using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial Content", menuName = "[ MINE Lab ]/Tutorial Content Data")]
public class TutorialContentScriptableObject : ScriptableObject
{
    #region [ Editor ]
    [CustomEditor(typeof(TutorialContentScriptableObject))]
    public class TutorialContentScriptableObjectEditor : KeiishkiiLib.CustomInspector<TutorialContentScriptableObject>
    {
        protected override void OnInspectorRender()
        {
            bool previousWordWrapSetting = EditorStyles.textField.wordWrap;
            EditorStyles.textField.wordWrap = true;
            KeiishkiiLib.InspectorUtility.BooleanField("Display Title:", ref _targetScript.displayTitle);
            if (_targetScript.displayTitle) EditorGUILayout.TextArea(_targetScript.title);
            
            KeiishkiiLib.InspectorUtility.Separator();
            KeiishkiiLib.InspectorUtility.BooleanField("Display Content:", ref _targetScript.displayContent);
            if (_targetScript.displayContent) EditorGUILayout.TextArea(_targetScript.content);
            
            KeiishkiiLib.InspectorUtility.Separator();
            KeiishkiiLib.InspectorUtility.BooleanField("Display Continue Condition:", ref _targetScript.displayContinueCondition);
            if (_targetScript.displayContinueCondition) EditorGUILayout.TextArea(_targetScript.continueCondition);
            EditorStyles.textField.wordWrap = previousWordWrapSetting;
        }
    }
    

    #endregion
    
    #region [ Serialised Fields ]
    public bool displayTitle;
    [TextArea(1,10000)] public string title;
    public bool displayContent;
    [TextArea(3,10000)] public string content;
    public bool displayContinueCondition;
    [TextArea(1,10000)] public string continueCondition;
    #endregion
}

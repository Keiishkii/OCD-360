using System;
using UnityEditor;
using UnityEngine;

namespace MINELab
{
    public class ProgramManager : MonoBehaviour
    {
        #region [ Editor ]
        [HideInInspector][SerializeField] private StateEnum _activeEditorState;
        
        [CustomEditor((typeof(ProgramManager)))]
        public class ProgramManagerEditor : KeiishkiiLib.CustomInspector<ProgramManager>
        {
            public override void OnInspectorGUI()
            {
                bool stateChanged = KeiishkiiLib.InspectorUtility.ChangeCheck(() =>
                    _targetScript._activeEditorState = (StateEnum)EditorGUILayout.EnumPopup("State:", _targetScript._activeEditorState));
                
                SerializedProperty programStateMachineSerialisedProperty = serializedObject.FindProperty("programStateMachine");
                SerializedProperty stateProperty = programStateMachineSerialisedProperty.FindPropertyRelative(_targetScript._activeEditorState switch
                {
                    StateEnum.ConsentConformation => "programStateConsentConfirmation",
                    StateEnum.Calibration => "programStateCalibration",
                    StateEnum.Tutorial => "programStateTutorial",
                    StateEnum.Experiment => "programStateExperiment",
                    StateEnum.End => "programStateEnd",
                    _ => throw new ArgumentOutOfRangeException()
                });

                if (stateChanged) stateProperty.isExpanded = true;
                EditorGUILayout.PropertyField(stateProperty);
                
                serializedObject.ApplyModifiedProperties();
                base.OnInspectorGUI();
            }
        }
        #endregion
        
        #region [ Serialsed Fields ]
        public ProgramStateMachine programStateMachine = new ();
        #endregion

        private void Start() => programStateMachine.Initialise(this);
    }

    public enum StateEnum
    {
        ConsentConformation,
        Calibration,
        Tutorial,
        Experiment,
        End
    }
}
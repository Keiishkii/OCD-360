using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MINELab
{
    public class ProgramManager : MonoBehaviour
    {
        #if UNITY_EDITOR
        #region [ Editor ]
        [HideInInspector][SerializeField] private StateEnum _activeEditorState;
        
        [CustomEditor((typeof(ProgramManager)))]
        public class ProgramManagerEditor : KeiishkiiLib.CustomInspector<ProgramManager>
        {
            public override void OnInspectorGUI()
            {
                KeiishkiiLib.InspectorUtility.HorizontalScope(() =>
                {
                    Color defaultColour = GUI.backgroundColor;
                    
                    GUI.backgroundColor = (_targetScript.programStateMachine.programStateConsentConfirmation.skipState) ? _targetScript._inactiveStates : _targetScript._activeStates;
                    KeiishkiiLib.InspectorUtility.Button("Consent Confirmation", true, () => { _targetScript.programStateMachine.programStateConsentConfirmation.skipState = !_targetScript.programStateMachine.programStateConsentConfirmation.skipState;});
                    
                    GUI.backgroundColor = (_targetScript.programStateMachine.programStateCalibration.skipState) ? _targetScript._inactiveStates : _targetScript._activeStates;
                    KeiishkiiLib.InspectorUtility.Button("Calibration", true, () => { _targetScript.programStateMachine.programStateCalibration.skipState = !_targetScript.programStateMachine.programStateCalibration.skipState;});
                    
                    GUI.backgroundColor = (_targetScript.programStateMachine.programStateTutorial.skipState) ? _targetScript._inactiveStates : _targetScript._activeStates;
                    KeiishkiiLib.InspectorUtility.Button("Tutorial", true, () => { _targetScript.programStateMachine.programStateTutorial.skipState = !_targetScript.programStateMachine.programStateTutorial.skipState;});
                    
                    GUI.backgroundColor = (_targetScript.programStateMachine.programStateExperiment.skipState) ? _targetScript._inactiveStates : _targetScript._activeStates;
                    KeiishkiiLib.InspectorUtility.Button("Experiment", true, () => { _targetScript.programStateMachine.programStateExperiment.skipState = !_targetScript.programStateMachine.programStateExperiment.skipState;}); 
                    
                    GUI.backgroundColor = (_targetScript.programStateMachine.programStateEnd.skipState) ? _targetScript._inactiveStates : _targetScript._activeStates;
                    KeiishkiiLib.InspectorUtility.Button("End", true, () => { _targetScript.programStateMachine.programStateEnd.skipState = !_targetScript.programStateMachine.programStateEnd.skipState;}); 
                    
                    GUI.backgroundColor = defaultColour;
                });
                
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
        #endif
        
        #region [ Serialsed Fields ]
        public ProgramStateMachine programStateMachine = new ();
        [SerializeField] private Color _activeStates;
        [SerializeField] private Color _inactiveStates;
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
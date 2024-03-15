using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MINELab
{
    [Serializable]
    public class ProgramStateTutorial : ProgramStateInterface
    {
        #region [ Serialised Fields ]
        [SerializeField] private GameObject _instructionUIGameObject;
        [SerializeField] private GameObject _affectScaleUIGameObject;
        [SerializeField] private InputActionReference _triggerPressInputActionReference;
        [SerializeField] private InputActionReference _touchPadPressInputActionReference;
        [SerializeField] private InputActionReference _editorRatingSkipInputActionReference;
        [SerializeField] private InputActionReference _rightJoystickInputActionReference;

        [SerializeField] private float _offsetLeneancy = 0;
        
        [SerializeField] private TutorialContentScriptableObject _ratingInstruction1PanelData;
        [SerializeField] private TutorialContentScriptableObject _ratingInstruction2PanelData;
        [SerializeField] private TutorialContentScriptableObject _ratingInstruction3PanelData;
        [SerializeField] private TutorialContentScriptableObject _ratingInstruction4PanelData;
        [SerializeField] private TutorialContentScriptableObject _ratingInstruction5PanelData;
        
        [SerializeField] private TutorialContentScriptableObject _practiceInstruction1PanelData;
        [SerializeField] private TutorialContentScriptableObject _practiceInstruction2PanelData;
        [SerializeField] private TutorialContentScriptableObject _practiceInstruction3PanelData;
        [SerializeField] private TutorialContentScriptableObject _practiceInstruction4PanelData;
        [SerializeField] private TutorialContentScriptableObject _practiceInstruction5PanelData;
        [SerializeField] private TutorialContentScriptableObject _practiceInstruction6PanelData;
        [SerializeField] private TutorialContentScriptableObject _practiceInstruction7PanelData;
        [SerializeField] private TutorialContentScriptableObject _practiceInstruction8PanelData;
        [SerializeField] private TutorialContentScriptableObject _practiceInstruction9PanelData;
        [SerializeField] private TutorialContentScriptableObject _practiceInstruction10PanelData;
        [SerializeField] private TutorialContentScriptableObject _practiceInstruction11PanelData;
        [SerializeField] private TutorialContentScriptableObject _practiceInstruction12PanelData;
        
        [SerializeField] private TutorialContentScriptableObject _mainInstructions1PanelData;
        [SerializeField] private TutorialContentScriptableObject _mainInstructions2PanelData;
        [SerializeField] private TutorialContentScriptableObject _mainInstructions3PanelData;
        [SerializeField] private TutorialContentScriptableObject _mainInstructions4PanelData;
        
        [SerializeField] private EnvironmentDisplayData _enviroment1Data;
        [SerializeField] private EnvironmentDisplayData _enviroment2Data;
        [SerializeField] private EnvironmentDisplayData _enviroment3Data;
        [SerializeField] private EnvironmentDisplayData _enviroment4Data;
        [SerializeField] private EnvironmentDisplayData _enviroment5Data;
        
        [SerializeField] private Vector2 _tutorial1InputTarget;
        [SerializeField] private Vector2 _tutorial2InputTarget;
        [SerializeField] private Vector2 _tutorial3InputTarget;
        [SerializeField] private Vector2 _tutorial4InputTarget;
        [SerializeField] private Vector2 _tutorial5InputTarget;
        [SerializeField] private Vector2 _tutorial6InputTarget;
        
        #endregion

        #region [ Unserialised Fields ]
        private InstructionUI _instructionUI;
        private IEnumerator _welcomeDisplayCoroutine;
        #endregion


        public override void StateInitialisation(ProgramManager programManager)
        {
            base.StateInitialisation(programManager);
            
            _instructionUI = _instructionUIGameObject.GetComponent<InstructionUI>();
        }

        public override void StateEnter()
        {
            if (skipState)
            {
                _programStateMachine.ActiveState = _programStateMachine.programStateExperiment;
                return;
            }

            _programManager.StartCoroutine(DisplayTutorial());
        }

        private IEnumerator DisplayTutorial()
        {
            bool loop = false;
            do
            {
                // Rating Instructions

                #region [ Rating Instructions 1 ]

                _instructionUIGameObject.SetActive(true);
                _instructionUI.SetPanelPosition(Vector3.forward);
                _instructionUI.SetPanelContent(_ratingInstruction1PanelData);
                yield return WaitForTriggerPress();

                #endregion

                #region [ Rating Instructions 2 ]

                _instructionUI.SetPanelContent(_ratingInstruction2PanelData);
                yield return WaitForTriggerPress();

                #endregion

                #region [ Rating Instructions 3 ]

                _instructionUI.SetPanelContent(_ratingInstruction3PanelData);
                yield return WaitForTriggerPress();

                #endregion

                #region [ Rating Instructions 4 ]

                _instructionUI.SetPanelContent(_ratingInstruction4PanelData);
                yield return WaitForTriggerPress();

                #endregion

                #region [ Rating Instructions 5 ]

                _affectScaleUIGameObject.SetActive(true);
                _instructionUI.SetPanelContent(_ratingInstruction5PanelData);
                yield return WaitForTriggerPress();

                #endregion

                // Practice Instructions

                #region [ Practice Instructions 1 ]

                _instructionUI.SetPanelContent(_practiceInstruction1PanelData);
                yield return WaitForTriggerPress();
                _instructionUIGameObject.SetActive(false);

                #endregion

                #region [ Practice Instructions 2 ]

                yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                {
                    _instructionUIGameObject.SetActive(true);
                    // Rotate canvas to face player?
                    _instructionUI.SetPanelContent(_practiceInstruction2PanelData);
                    EnvironmentManager.Instance.SetEnvironment(_enviroment1Data.environmentMaterial, _enviroment1Data.environmentYRotationOffset);
                });
                yield return WaitForTriggerPress();

                #endregion

                #region [ Practice Instructions 3 ]

                _instructionUI.SetPanelContent(_practiceInstruction3PanelData);
                yield return WaitForJoystickInput(_tutorial1InputTarget);

                #endregion

                #region [ Practice Instructions 4 ]

                _instructionUI.SetPanelContent(_practiceInstruction4PanelData);
                yield return WaitForJoystickInput(_tutorial2InputTarget);
                _instructionUIGameObject.SetActive(false);

                #endregion

                #region [ Practice Instructions 5 ]

                yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                {
                    _instructionUIGameObject.SetActive(true);
                    // Rotate canvas to face player?
                    _instructionUI.SetPanelContent(_practiceInstruction5PanelData);
                    EnvironmentManager.Instance.SetEnvironment(_enviroment2Data.environmentMaterial, _enviroment2Data.environmentYRotationOffset);
                });
                yield return WaitForTriggerPress();

                #endregion

                #region [ Practice Instructions 6 ]

                _instructionUI.SetPanelContent(_practiceInstruction6PanelData);
                yield return WaitForJoystickInput(_tutorial3InputTarget);

                #endregion

                #region [ Practice Instructions 7 ]

                _instructionUI.SetPanelContent(_practiceInstruction7PanelData);
                yield return WaitForTriggerPress();
                _instructionUIGameObject.SetActive(false);

                #endregion

                #region [ Practice Instructions 8 ]

                yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                {
                    _instructionUIGameObject.SetActive(true);
                    // Rotate canvas to face player?
                    _instructionUI.SetPanelContent(_practiceInstruction8PanelData);
                    EnvironmentManager.Instance.SetEnvironment(_enviroment3Data.environmentMaterial, _enviroment3Data.environmentYRotationOffset);
                });
                yield return WaitForJoystickInput(_tutorial4InputTarget);
                _instructionUIGameObject.SetActive(false);

                #endregion

                #region [ Practice Instructions 9 ]

                yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                {
                    _instructionUIGameObject.SetActive(true);
                    // Rotate canvas to face player?
                    _instructionUI.SetPanelContent(_practiceInstruction9PanelData);
                    EnvironmentManager.Instance.SetEnvironment(_enviroment4Data.environmentMaterial, _enviroment4Data.environmentYRotationOffset);
                });
                yield return WaitForJoystickInput(_tutorial5InputTarget);

                #endregion

                #region [ Practice Instructions 10 ]

                _instructionUI.SetPanelContent(_practiceInstruction10PanelData);
                yield return WaitForTriggerPress();
                _instructionUIGameObject.SetActive(false);

                #endregion

                #region [ Practice Instructions 11 ]

                yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                {
                    _instructionUIGameObject.SetActive(true);
                    // Rotate canvas to face player?
                    _instructionUI.SetPanelContent(_practiceInstruction11PanelData);
                    EnvironmentManager.Instance.SetEnvironment(_enviroment5Data.environmentMaterial, _enviroment5Data.environmentYRotationOffset);
                });
                yield return WaitForJoystickInput(_tutorial5InputTarget);
                _instructionUIGameObject.SetActive(false);

                #endregion

                #region [ Practice Instructions 12 ]

                yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                {
                    _instructionUIGameObject.SetActive(true);
                    // Rotate canvas to face player?
                    _instructionUI.SetPanelContent(_practiceInstruction12PanelData);
                    _affectScaleUIGameObject.SetActive(false);
                    EnvironmentManager.Instance.Return();
                });
                yield return WaitForChoice((choice) =>
                {
                    loop = (choice != InputChoice.Trigger);
                });
                #endregion
            } while (loop);

            // Main Instructions

            #region [ Main Instructions 1 ]
            _instructionUI.SetPanelContent(_mainInstructions1PanelData);
            yield return WaitForTriggerPress();
            #endregion
            
            #region [ Main Instructions 2 ]
            _instructionUI.SetPanelContent(_mainInstructions2PanelData);
            yield return WaitForTriggerPress();
            #endregion
            
            #region [ Main Instructions 3 ]
            _instructionUI.SetPanelContent(_mainInstructions3PanelData);
            yield return WaitForTriggerPress();
            #endregion
            
            #region [ Main Instructions 4 ]
            _instructionUI.SetPanelContent(_mainInstructions4PanelData);
            yield return WaitForTriggerPress();
            _instructionUIGameObject.SetActive(false);
            #endregion    
            
            _programStateMachine.ActiveState = _programStateMachine.programStateExperiment;
        }

        private IEnumerator WaitForChoice(Action<InputChoice> callback)
        {
            InputChoice choice = InputChoice.None;
            yield return new WaitUntil(() =>
            {
                if (_triggerPressInputActionReference.action.WasPressedThisFrame())
                {
                    choice = InputChoice.Trigger;
                    return true;
                }
                
                if (_touchPadPressInputActionReference.action.WasPressedThisFrame())
                {
                    choice = InputChoice.TouchPad;
                    return true;
                }
                
                return false;
            });
            yield return null;
            callback(choice);
        }
        
        private IEnumerator WaitForTriggerPress()
        {
            yield return new WaitUntil(() => _triggerPressInputActionReference.action.WasPressedThisFrame());
            yield return null;
        }

        private IEnumerator WaitForJoystickInput(Vector2 target)
        {
            AffectScale.Instance.SetTargetPosition(target);
            AffectScale.Instance.SetTargetVisible(true);
            yield return new WaitUntil(() =>
            {
                if (_editorRatingSkipInputActionReference.action.WasPressedThisFrame()) return true; 
                Vector2 value = _rightJoystickInputActionReference.action.ReadValue<Vector2>();

                return (Vector3.SqrMagnitude(target - value) < _offsetLeneancy * _offsetLeneancy);
            });
            AffectScale.Instance.SetTargetVisible(false);
            yield return null;
        }
    }
}

public enum InputChoice
{
    None,
    Trigger,
    TouchPad
}
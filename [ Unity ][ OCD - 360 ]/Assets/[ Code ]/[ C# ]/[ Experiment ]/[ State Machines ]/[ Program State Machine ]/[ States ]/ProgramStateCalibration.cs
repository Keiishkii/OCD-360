using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace MINELab
{
    [Serializable]
    public class ProgramStateCalibration : ProgramStateInterface
    {
        #region [ Serialised Fields ]
        [SerializeField] private GameObject _instructionUIGameObject;
        [SerializeField] private InputActionReference _triggerPressInputActionReference;
        
        [SerializeField] private TutorialContentScriptableObject _welcomePanelData;
        [SerializeField] private TutorialContentScriptableObject _studyIntroduction1PanelData;
        [SerializeField] private TutorialContentScriptableObject _studyIntroduction2PanelData;
        [SerializeField] private TutorialContentScriptableObject _emteqCalibrationPanelData;
        #endregion

        #region [ Unserialised Fields ]
        private EmteqCalibrator _emteqCalibrator;
        private LargeScreenUI _largeScreenUI;
        
        private InstructionUI _instructionUI;
        private IEnumerator _welcomeDisplayCoroutine;
        #endregion


        public override void StateInitialisation(ProgramManager programManager)
        {
            base.StateInitialisation(programManager);

            _emteqCalibrator = Object.FindObjectOfType<EmteqCalibrator>();
            _largeScreenUI = Object.FindObjectOfType<LargeScreenUI>();
            _instructionUI = _instructionUIGameObject.GetComponent<InstructionUI>();
        }

        public override void StateEnter()
        {        
            if (skipState)
            {
                _programStateMachine.ActiveState = _programStateMachine.programStateTutorial;
                return;
            }
            
            _programManager.StartCoroutine(DisplayWelcomePanels());
        }

        private IEnumerator DisplayWelcomePanels()
        {
            _instructionUIGameObject.SetActive(true);
            _instructionUI.SetPanelPosition(Vector3.forward);
            
            _instructionUI.SetPanelContent(_welcomePanelData);
            yield return WaitForTriggerPress();
            
            _instructionUI.SetPanelContent(_studyIntroduction1PanelData);
            yield return WaitForTriggerPress();
            
            _instructionUI.SetPanelContent(_studyIntroduction2PanelData);
            yield return WaitForTriggerPress();
            
            _instructionUI.SetPanelContent(_emteqCalibrationPanelData);
            yield return WaitForTriggerPress();
            
            _instructionUIGameObject.SetActive(false);
            
            
            _largeScreenUI.DisplayActive = true;
            yield return _programManager.StartCoroutine(_emteqCalibrator.RecordHeartbeatBaseline());
            yield return _programManager.StartCoroutine(_emteqCalibrator.RecordExpressions());
            _largeScreenUI.DisplayActive = false;
            
            
            _programStateMachine.ActiveState = _programStateMachine.programStateTutorial;
        }

        private IEnumerator WaitForTriggerPress()
        {
            yield return new WaitUntil(() => _triggerPressInputActionReference.action.WasPressedThisFrame());
            yield return null;
        }
    }
}
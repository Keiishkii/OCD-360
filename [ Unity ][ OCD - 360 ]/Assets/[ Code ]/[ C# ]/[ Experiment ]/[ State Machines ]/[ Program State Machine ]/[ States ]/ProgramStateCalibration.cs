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
        [SerializeField] private TutorialUIContentManager tutorialUIContentManager = new ();
        #endregion

        #region [ Unserialised Fields ]
        private EmteqCalibrator _emteqCalibrator;
        private LargeScreenUI _largeScreenUI;
        private DisplayController _displayController;
        private InstructionUI _instructionUI;
        #endregion

        
        
        public override void StateInitialisation(ProgramManager programManager)
        {
            base.StateInitialisation(programManager);

            _emteqCalibrator = Object.FindObjectOfType<EmteqCalibrator>();
            _displayController = Object.FindObjectOfType<DisplayController>();
            _largeScreenUI = Object.FindObjectOfType<LargeScreenUI>();
            _instructionUI = Object.FindObjectOfType<InstructionUI>();
        }

        public override void StateEnter()
        {        
            #if UNITY_EDITOR
            if (skipState)
            {
                _programStateMachine.ActiveState = _programStateMachine.programStateTutorial;
                return;
            }
            #endif
            
            _programManager.StartCoroutine(DisplayWelcomePanels());
        }

        private IEnumerator DisplayWelcomePanels()
        {
            _instructionUI.Visible = true;
            _instructionUI.SetPanelPosition(Vector3.forward);
            /*
            #region [ Introduction 1 ]
            _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 1 ][ Introduction ]"));
            _displayController.Visible = true;
            _displayController.HighlightTrigger();
            yield return null;
            yield return InputManager.Instance.WaitForTriggerPress();
            #endregion
            
            #region [ Introduction 2 ]
            _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 2 ][ Introduction ]"));
            yield return InputManager.Instance.WaitForTriggerPress();
            #endregion
            
            #region [ Introduction 3 ]
            _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 3 ][ Introduction ]"));
            _displayController.HighlightTouchPad();
            yield return InputManager.Instance.WaitForTouchpadPress();
            #endregion
            */

            _displayController.Visible = false;
            _displayController.ResetHighlight();
            
            #region [ Study Introduction 1 ]
            _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 1 ][ Study Introduction ]"));
            yield return InputManager.Instance.WaitForTriggerPress();
            #endregion
            
            #region [ Study Introduction 2 ]
            _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 2 ][ Study Introduction ]"));
            yield return InputManager.Instance.WaitForTriggerPress();
            #endregion
            
            #region [ Emteq Calibration ]
            _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ Emteq Calibration ]"));
            yield return InputManager.Instance.WaitForTriggerPress();
            #endregion
            
            _instructionUI.Visible = false;
            _largeScreenUI.DisplayActive = true;
            
            yield return _programManager.StartCoroutine(_emteqCalibrator.RecordHeartbeatBaseline());
            yield return _programManager.StartCoroutine(_emteqCalibrator.RecordExpressions());
            _largeScreenUI.DisplayActive = false;
            
            
            _programStateMachine.ActiveState = _programStateMachine.programStateTutorial;
        }
    }
}
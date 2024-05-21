using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MINELab
{
    [Serializable]
    public class ProgramStateTutorial : ProgramStateInterface
    {
        #region [ Serialised Fields ]
        [SerializeField] private TutorialUIContentManager tutorialUIContentManager = new ();
        [SerializeField] private TutorialEnvironmentContentManager tutorialEnvironmentContentManager = new ();
        [SerializeField] private TutorialRatingInputManager tutorialRatingManager = new ();
        #endregion

        #region [ Unserialised Fields ]
        private InstructionUI _instructionUI;
        private AffectScale _affectScale;
        private IEnumerator _welcomeDisplayCoroutine;
        #endregion


        public override void StateInitialisation(ProgramManager programManager)
        {
            base.StateInitialisation(programManager);
            
            _instructionUI = Object.FindObjectOfType<InstructionUI>();
            _affectScale = Object.FindObjectOfType<AffectScale>();
        }

        public override void StateEnter()
        {
            #if UNITY_EDITOR
            if (skipState)
            {
                _programStateMachine.ActiveState = _programStateMachine.programStateExperiment;
                return;
            }
            #endif

            _programManager.StartCoroutine(DisplayTutorial());
        }

        private IEnumerator DisplayTutorial()
        {
            bool loop = false;
            do
            {
                // Rating Instructions
                #region [ Rating Instructions 1 ]
                _instructionUI.Visible = true;
                _instructionUI.SetPanelPosition(Vector3.forward);
                _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 1 ][ Rating Instructions ]"));
                yield return InputManager.Instance.WaitForTriggerPress();
                #endregion
                
                #region [ Rating Instructions 2 ]
                _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 2 ][ Rating Instructions ]"));
                yield return InputManager.Instance.WaitForTriggerPress();
                #endregion

                #region [ Rating Instructions 3 ]
                _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 3 ][ Rating Instructions ]"));
                yield return InputManager.Instance.WaitForTriggerPress();
                #endregion

                #region [ Rating Instructions 4 ]
                _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 4 ][ Rating Instructions ]"));
                yield return InputManager.Instance.WaitForTriggerPress();
                #endregion

                #region [ Rating Instructions 5 ]
                _affectScale.Visible = true;
                _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 5 ][ Rating Instructions ]"));
                yield return InputManager.Instance.WaitForTriggerPress();
                #endregion

                // Practice Instructions
                #region [ Practice Instructions 1 ]
                _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 1 ][ Practice Instructions ]"));
                yield return InputManager.Instance.WaitForTriggerPress();
                _instructionUI.Visible = false;
                #endregion

                #region [ Practice Instructions 2 ]
                yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                {
                    _instructionUI.Visible = true;
                    _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 2 ][ Practice Instructions ]"));
                    EnvironmentManager.Instance.SetEnvironment(tutorialEnvironmentContentManager.Content("[ Beach ][ 0 ]"));
                });
                yield return InputManager.Instance.WaitForTriggerPress();
                #endregion

                #region [ Practice Instructions 3 ]
                _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 3 ][ Practice Instructions ]"));
                yield return WaitForJoystickInput(tutorialRatingManager.Content("[ 1 ][ Rating ]"));
                #endregion

                #region [ Practice Instructions 4 ]
                _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 4 ][ Practice Instructions ]"));
                yield return WaitForJoystickInput(tutorialRatingManager.Content("[ 2 ][ Rating ]"));
                _instructionUI.Visible = false;
                #endregion

                #region [ Practice Instructions 5 ]
                yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                {
                    _instructionUI.Visible = true;
                    _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 5 ][ Practice Instructions ]"));
                    EnvironmentManager.Instance.SetEnvironment(tutorialEnvironmentContentManager.Content("[ Living Room ][ 0 ]"));
                });
                yield return InputManager.Instance.WaitForTriggerPress();
                #endregion

                #region [ Practice Instructions 6 ]
                _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 6 ][ Practice Instructions ]"));
                yield return WaitForJoystickInput(tutorialRatingManager.Content("[ 3 ][ Rating ]"));
                #endregion

                #region [ Practice Instructions 7 ]
                _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 7 ][ Practice Instructions ]"));
                yield return InputManager.Instance.WaitForTriggerPress();
                _instructionUI.Visible = false;
                #endregion

                #region [ Practice Instructions 8 ]
                yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                {
                    _instructionUI.Visible = true;
                    _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 8 ][ Practice Instructions ]"));
                    EnvironmentManager.Instance.SetEnvironment(tutorialEnvironmentContentManager.Content("[ Living Room ][ 180 ]"));
                });
                yield return WaitForJoystickInput(tutorialRatingManager.Content("[ 4 ][ Rating ]"));
                _instructionUI.Visible = false;
                #endregion

                #region [ Practice Instructions 9 ]
                yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                {
                    _instructionUI.Visible = true;
                    _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 9 ][ Practice Instructions ]"));
                    EnvironmentManager.Instance.SetEnvironment(tutorialEnvironmentContentManager.Content("[ Office ][ 0 ]"));
                });
                yield return WaitForJoystickInput(tutorialRatingManager.Content("[ 5 ][ Rating ]"));
                #endregion

                #region [ Practice Instructions 10 ]
                _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 10 ][ Practice Instructions ]"));
                yield return InputManager.Instance.WaitForTriggerPress();
                _instructionUI.Visible = false;
                #endregion

                #region [ Practice Instructions 11 ]
                yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                {
                    _instructionUI.Visible = true;
                    _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 11 ][ Practice Instructions ]"));
                    EnvironmentManager.Instance.SetEnvironment(tutorialEnvironmentContentManager.Content("[ Office ][ 180 ]"));
                });
                yield return WaitForJoystickInput(tutorialRatingManager.Content("[ 6 ][ Rating ]"));
                _instructionUI.Visible = false;
                #endregion

                #region [ Practice Instructions 12 ]
                yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                {
                    _instructionUI.Visible = true;
                    _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 12 ][ Practice Instructions ]"));
                    _affectScale.Visible = false;
                    EnvironmentManager.Instance.Return();
                });
                
                yield return InputManager.Instance.WaitForChoice((choice) =>
                {
                    loop = (choice != InputChoice.Trigger);
                });
                #endregion
                
            } while (loop);

            // Main Instructions
            #region [ Main Instructions 1 ]
            _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 1 ][ Main Instructions ]"));
            yield return InputManager.Instance.WaitForTriggerPress();
            #endregion
            
            #region [ Main Instructions 2 ]
            _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 2 ][ Main Instructions ]"));
            yield return InputManager.Instance.WaitForTriggerPress();
            #endregion
            
            #region [ Main Instructions 3 ]
            _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 3 ][ Main Instructions ]"));
            yield return InputManager.Instance.WaitForTriggerPress();
            #endregion
            
            #region [ Main Instructions 4 ]
            _instructionUI.SetPanelContent(tutorialUIContentManager.Content("[ 4 ][ Main Instructions ]"));
            yield return InputManager.Instance.WaitForTriggerPress();
            _instructionUI.Visible = false;
            #endregion    
            
            _programStateMachine.ActiveState = _programStateMachine.programStateExperiment;
        }

        private IEnumerator WaitForJoystickInput(Vector2 target)
        {
            float squareLeniency = tutorialRatingManager.offsetLeniency * tutorialRatingManager.offsetLeniency;

            AffectScale.Instance.SetTargetPosition(target);
            AffectScale.Instance.SetTargetVisible(true);
            yield return new WaitUntil(() =>
            {
                if (InputManager.Instance.ratingSkip.action.WasPressedThisFrame()) return true;
                
                Vector2 value = InputManager.Instance.rightJoystick.action.ReadValue<Vector2>();
                return (Vector3.SqrMagnitude(target - value) < squareLeniency);
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
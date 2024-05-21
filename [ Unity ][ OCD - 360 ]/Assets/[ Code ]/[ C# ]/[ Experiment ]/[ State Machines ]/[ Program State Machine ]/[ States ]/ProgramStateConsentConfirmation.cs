using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Object = UnityEngine.Object;

namespace MINELab
{
    [Serializable]
    public class ProgramStateConsentConfirmation : ProgramStateInterface
    {
        #region [ Serialised Fields ]
        [SerializeField] private XRInteractorLineVisual _leftHandRayVisual;
        [SerializeField] private XRInteractorLineVisual _rightHandRayVisual;
        #endregion

        #region [ Unserialised Fields ]
        private ConsentConfirmationUI _consentConfirmationUI;
        #endregion

        

        public override void StateInitialisation(ProgramManager programManager)
        {
            base.StateInitialisation(programManager);
            _consentConfirmationUI = Object.FindObjectOfType<ConsentConfirmationUI>();
        }

        public override void StateEnter()
        {
            _leftHandRayVisual.enabled = false;
            _rightHandRayVisual.enabled = true;
            
            #if UNITY_EDITOR
            if (skipState)
            {
                _programStateMachine.ActiveState = _programStateMachine.programStateCalibration;
                return;
            }
            #endif
            
            _consentConfirmationUI.Visible = true;
        }

        public override void StateExit()
        {
            _consentConfirmationUI.Visible = false;
            _rightHandRayVisual.enabled = false;
            _leftHandRayVisual.enabled = false;
        }
    }
}
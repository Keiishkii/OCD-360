using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace MINELab
{
    [Serializable]
    public class ProgramStateConsentConfirmation : ProgramStateInterface
    {
        #region [ Serialised Fields ]
        [SerializeField] private GameObject _consentConfirmationUIGameObject;
        [SerializeField] private XRInteractorLineVisual _leftHandRayVisual;
        [SerializeField] private XRInteractorLineVisual _rightHandRayVisual;
        #endregion

        #region [ Unserialised Fields ]
        #endregion
        
        
        
        public override void StateEnter()
        {
            _leftHandRayVisual.enabled = false;
            _rightHandRayVisual.enabled = true;
            
            if (skipState)
            {
                _programStateMachine.ActiveState = _programStateMachine.programStateCalibration;
                return;
            }
            
            _consentConfirmationUIGameObject.SetActive(true);
        }

        public override void StateExit()
        {
            _consentConfirmationUIGameObject.SetActive(false);
            _rightHandRayVisual.enabled = false;
            _leftHandRayVisual.enabled = false;
        }
    }
}
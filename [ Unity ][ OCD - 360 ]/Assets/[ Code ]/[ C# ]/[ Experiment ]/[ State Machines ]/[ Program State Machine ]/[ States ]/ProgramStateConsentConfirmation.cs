using System;
using UnityEngine;

namespace MINELab
{
    [Serializable]
    public class ProgramStateConsentConfirmation : ProgramStateInterface
    {
        #region [ Serialised Fields ]
        [SerializeField] private GameObject _consentConfirmationUIGameObject;
        #endregion

        #region [ Unserialised Fields ]
        #endregion
        
        
        
        public override void StateEnter()
        {
            if (skipState)
            {
                _programStateMachine.ActiveState = _programStateMachine.programStateCalibration;
                return;
            }
            
            _consentConfirmationUIGameObject.SetActive(true);
        }

        public override void StateExit() => _consentConfirmationUIGameObject.SetActive(false);
    }
}
using System;
using UnityEngine;

namespace MINELab
{
    [Serializable]
    public class ProgramStateMachine
    {
        #region [ States ]
        private ProgramStateInterface _activeState;
        public ProgramStateInterface ActiveState
        {
            get => _activeState;
            set
            {
                _activeState?.StateExit();
                _activeState = value;            
                _activeState.StateEnter();
                Debug.Log($"{_activeState.stateName} State Entered");
            }
        }

        public ProgramStateConsentConfirmation programStateConsentConfirmation = new ();
        public ProgramStateCalibration programStateCalibration = new ();
        public ProgramStateTutorial programStateTutorial = new ();
        public ProgramStateExperiment programStateExperiment = new ();
        public ProgramStateEnd programStateEnd = new ();
        #endregion

        
        
        public void Initialise(ProgramManager programManager)
        {
            programStateConsentConfirmation.StateInitialisation(programManager);
            programStateCalibration.StateInitialisation(programManager);
            programStateTutorial.StateInitialisation(programManager);
            programStateExperiment.StateInitialisation(programManager);
            programStateEnd.StateInitialisation(programManager);
            
            ActiveState = programStateConsentConfirmation;
        }
    }   
}
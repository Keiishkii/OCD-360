using System;
using UnityEngine;

namespace MINELab
{
    [Serializable]
    public abstract class ProgramStateInterface
    {
        #region [ Serialised Fields ]
        public string stateName;
        [HideInInspector] public bool skipState = false;
        #endregion

        #region [ Unserialised Fields ]
        [NonSerialized] protected ProgramManager _programManager;
        [NonSerialized] protected ProgramStateMachine _programStateMachine;
        #endregion



        public virtual void StateInitialisation(ProgramManager programManager)
        {
            _programManager = programManager;
            _programStateMachine = programManager.programStateMachine;
        }

        public virtual void StateEnter() {}
        public virtual void StateExit() {}
    }
}
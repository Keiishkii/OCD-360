using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MINELab
{
    [Serializable]
    public class ProgramStateEnd : ProgramStateInterface
    {
        #region [ Serialised Fields ]
        [SerializeField] private GameObject _endScreenUIGameObject;
        #endregion

        #region [ Unserialised Fields ]
        private IEnumerator _endStateCoroutine;
        #endregion
        
        
        public override void StateEnter()
        {
            #if UNITY_EDITOR
            if (skipState)
            {
                _programStateMachine.ActiveState = _programStateMachine.programStateEnd;
                return;
            }
            #endif

            _programManager.StartCoroutine(_endStateCoroutine = EndStateCoroutine());
        }

        public override void StateExit()
        {
            if (_endStateCoroutine != null) _programManager.StopCoroutine(_endStateCoroutine);
        }


        private IEnumerator EndStateCoroutine()
        {
            _endScreenUIGameObject.SetActive(true);
            
            yield return new WaitForSeconds(30);
            Application.Quit();
        }
    }
}
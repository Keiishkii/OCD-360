using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MINELab
{
    [Serializable]
    public class ProgramStateExperiment : ProgramStateInterface
    {
        #region [ Serialised Fields ]
        [SerializeField] private GameObject _affectScaleUIGameObject;
        [SerializeField] private List<EnvironmentDisplayData> _enviromentList = new ();
        [SerializeField] private float _environmentHoldDuration;
        #endregion

        #region [ Unserialised Feilds ]
        private IEnumerator _experimentCoroutine;
        #endregion


        
        
        
        public override void StateEnter()
        {
            if (skipState)
            {
                _programStateMachine.ActiveState = _programStateMachine.programStateEnd;
                return;
            }

            _programManager.StartCoroutine(_experimentCoroutine = ExperimentCoroutine());
        }

        public override void StateExit()
        {
            if (_experimentCoroutine != null) _programManager.StopCoroutine(_experimentCoroutine);
        }


        private IEnumerator ExperimentCoroutine()
        {
            yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
            {
                _affectScaleUIGameObject.SetActive(true);
                
                EnvironmentDisplayData data = _enviromentList[0];
                EnvironmentManager.Instance.SetEnvironment(data.environmentMaterial, data.environmentYRotationOffset);
            });
            
            yield return new WaitForSeconds(_environmentHoldDuration);
            
            for (int i = 1; i < _enviromentList.Count; i++)
            {
                int environmentIndex = i;
                yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                {
                    EnvironmentDisplayData data = _enviromentList[environmentIndex];
                    EnvironmentManager.Instance.SetEnvironment(data.environmentMaterial, data.environmentYRotationOffset);
                });
                
                yield return new WaitForSeconds(_environmentHoldDuration);
            }
            
            yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
            {
                EnvironmentManager.Instance.Return();
                _affectScaleUIGameObject.SetActive(false);
            });
            
            _programStateMachine.ActiveState = _programStateMachine.programStateEnd;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MINELab
{
    [Serializable]
    public class ProgramStateExperiment : ProgramStateInterface
    {
        #region [ Serialised Fields ]
        [SerializeField] private GameObject _affectScaleUIGameObject;
        [SerializeField] private List<Block> _blocks;
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
            for (int blockIndex = 0; blockIndex < _blocks.Count; blockIndex++)
            {
                int unmodifiedBlockIndex = blockIndex;
                for (int trialIndex = 0; trialIndex < _blocks[blockIndex].trials.Count; trialIndex++)
                {
                    int unmodifiedTrialIndex = trialIndex;
                    Trial trialData = _blocks[unmodifiedBlockIndex].trials[unmodifiedTrialIndex];

                    // Display Trial From View 1
                    float environmentYRotationOffset = trialData.environmentYRotationOffset + (Random.Range(45f, 135f) * (Random.Range(0, 1) == 0 ? 1 : -1));
                    yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                    {
                        if (unmodifiedBlockIndex == 0 && unmodifiedTrialIndex == 0) _affectScaleUIGameObject.SetActive(true);
                        EnvironmentManager.Instance.SetEnvironment(trialData.environmentMaterial, environmentYRotationOffset);
                        
                        Debug.Log($"Environment Rotation: {environmentYRotationOffset}");
                    });
                
                    yield return new WaitForSeconds(_environmentHoldDuration);
                    
                    // Display Trial From View 2
                    float mirroredEnvironmentYRotationOffset = environmentYRotationOffset + 180;
                    yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                    {
                        if (unmodifiedBlockIndex == 0 && unmodifiedTrialIndex == 0) _affectScaleUIGameObject.SetActive(true);
                        EnvironmentManager.Instance.SetEnvironment(trialData.environmentMaterial, mirroredEnvironmentYRotationOffset);
                        
                        Debug.Log($"Mirrored Environment Rotation: {mirroredEnvironmentYRotationOffset}");
                    });
                
                    yield return new WaitForSeconds(_environmentHoldDuration);
                }
                
                // Display Block Beach
                List<EnvironmentDataScriptableObject> beachEnvironments = _blocks[unmodifiedBlockIndex]._beachEnvironments;
                EnvironmentDataScriptableObject beachData = beachEnvironments[Random.Range(0, beachEnvironments.Count)];
                yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                {
                    EnvironmentManager.Instance.SetEnvironment(beachData, 0);
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
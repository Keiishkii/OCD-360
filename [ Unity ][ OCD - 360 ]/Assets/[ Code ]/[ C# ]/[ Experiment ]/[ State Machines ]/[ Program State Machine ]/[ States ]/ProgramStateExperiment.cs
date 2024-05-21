using System;
using System.Collections;
using System.Collections.Generic;
using EmteqLabs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MINELab
{
    [Serializable]
    public class ProgramStateExperiment : ProgramStateInterface
    {
        #region [ Serialised Fields ]
        [SerializeField] private GameObject _affectScaleUIGameObject;
        [SerializeField] private List<BlockDataScriptableObject> _blocks;
        [SerializeField] private BeachDataScriptableObject _beachData;
        [SerializeField] private float _editorEnvironmentHoldDuration;
        [SerializeField] private float _environmentHoldDuration;
        #endregion

        #region [ Unserialised Feilds ]
        private IEnumerator _experimentCoroutine;
        #endregion


        
        
        
        public override void StateEnter()
        {
            #if UNITY_EDITOR
            if (skipState)
            {
                _programStateMachine.ActiveState = _programStateMachine.programStateEnd;
                return;
            }
            _environmentHoldDuration = _editorEnvironmentHoldDuration;
            #endif
            
            _programManager.StartCoroutine(_experimentCoroutine = ExperimentCoroutine());
        }

        public override void StateExit()
        {
            if (_experimentCoroutine != null) _programManager.StopCoroutine(_experimentCoroutine);
        }

        private IEnumerator ExperimentCoroutine()
        {
            DataManager.Instance.experimentData = new ExperimentData { experimentStartTime = DateTime.Now };
            EmteqManager.SetDataPoint($"Experiment Start");
            
            Dictionary<(int, int), float> trialYRotationOffsetDictionary = new ();
            for (int blockIndex = 0; blockIndex < _blocks.Count; blockIndex++)
            {
                BlockData blockOutputData = new ();
                DataManager.Instance.experimentData.blockData.Add(blockOutputData);
                
                int unmodifiedBlockIndex = blockIndex;
                
                for (int trialIndex = 0; trialIndex < _blocks[blockIndex].trials.Count; trialIndex++)
                {
                    TrialData trialOutputData = new ();
                    blockOutputData.trialData.Add(trialOutputData);
                    
                    int unmodifiedTrialIndex = trialIndex;
                    
                    // Display Beach Scene
                    Trial trialData = _blocks[unmodifiedBlockIndex].trials[unmodifiedTrialIndex];
                    float environmentYRotationOffset = trialData.environmentYRotationOffset + (Random.Range(45f, 135f) * (Random.Range(0, 1) == 0 ? 1 : -1));
                    trialYRotationOffsetDictionary.Add((unmodifiedBlockIndex, unmodifiedTrialIndex), environmentYRotationOffset);
                    trialOutputData.rotationOffset = environmentYRotationOffset;
                    
                    yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                    {
                        if (unmodifiedBlockIndex == 0 && unmodifiedTrialIndex == 0) _affectScaleUIGameObject.SetActive(true);
                        EnvironmentManager.Instance.SetEnvironment(trialData.environmentMaterial, environmentYRotationOffset);
                        EmteqManager.SetDataPoint($"Block: {unmodifiedBlockIndex} - Trial: {unmodifiedTrialIndex} - Rotation Offset: {environmentYRotationOffset}");
                    });

                    string scaleSelection = "";
                    for (float timeElapsed = 0; timeElapsed < _environmentHoldDuration; timeElapsed += Time.deltaTime)
                    {
                        Vector2 value = InputManager.Instance.RightJoystickValue;
                        trialOutputData.affectScaleValues.Add(value);

                        string activeSelection = ConvertScale(value);
                        if (!scaleSelection.Equals(activeSelection))
                        {
                            scaleSelection = activeSelection;
                            EmteqManager.SetDataPoint($"Affect Scale Value: {scaleSelection}");
                        }
                        
                        yield return null;
                    }
                    
                    EmteqManager.SetDataPoint($"Scene End");
                    
                    // Display Beach Scene
                    List<EnvironmentDataScriptableObject> beachEnvironments = _beachData.environments;
                    int randomisedBeachIndex = Random.Range(0, beachEnvironments.Count);
                    EnvironmentDataScriptableObject beachData = beachEnvironments[randomisedBeachIndex];
                    yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                    {
                        EmteqManager.SetDataPoint($"Displaying Beach: {randomisedBeachIndex}");
                        EnvironmentManager.Instance.SetEnvironment(beachData, 0);
                    }); 
                    
                    yield return new WaitForSeconds(_environmentHoldDuration);
                    EmteqManager.SetDataPoint($"Scene End");
                }
            }
            
            for (int blockIndex = 0; blockIndex < _blocks.Count; blockIndex++)
            {
                BlockData blockOutputData = new ();
                DataManager.Instance.experimentData.blockData.Add(blockOutputData);
                
                int unmodifiedBlockIndex = blockIndex;
                
                for (int trialIndex = 0; trialIndex < _blocks[blockIndex].trials.Count; trialIndex++)
                {
                    TrialData trialOutputData = new ();
                    blockOutputData.trialData.Add(trialOutputData);
                    
                    int unmodifiedTrialIndex = trialIndex;

                    // Display Beach Scene
                    Trial trialData = _blocks[unmodifiedBlockIndex].trials[unmodifiedTrialIndex];
                    float environmentYRotationOffset = trialYRotationOffsetDictionary[(unmodifiedBlockIndex, unmodifiedTrialIndex)] + 180;
                    trialOutputData.rotationOffset = environmentYRotationOffset;
                    
                    yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                    {
                        if (unmodifiedBlockIndex == 0 && unmodifiedTrialIndex == 0) _affectScaleUIGameObject.SetActive(true);
                        EnvironmentManager.Instance.SetEnvironment(trialData.environmentMaterial, environmentYRotationOffset);
                        EmteqManager.SetDataPoint($"Block: {unmodifiedBlockIndex} - Trial: {unmodifiedTrialIndex} - Rotation Offset: {environmentYRotationOffset}");
                    }); 

                    string scaleSelection = "";
                    for (float timeElapsed = 0; timeElapsed < _environmentHoldDuration; timeElapsed += Time.deltaTime)
                    {
                        Vector2 value = InputManager.Instance.RightJoystickValue;
                        trialOutputData.affectScaleValues.Add(value);

                        string activeSelection = ConvertScale(value);
                        if (!scaleSelection.Equals(activeSelection))
                        {
                            scaleSelection = activeSelection;
                            EmteqManager.SetDataPoint($"Affect Scale Value: {scaleSelection}");
                        }
                        
                        yield return null;
                    }
                    EmteqManager.SetDataPoint($"Scene End");
                    
                    // Display Beach Scene
                    List<EnvironmentDataScriptableObject> beachEnvironments = _beachData.environments;
                    int randomisedBeachIndex = Random.Range(0, beachEnvironments.Count);
                    EnvironmentDataScriptableObject beachData = beachEnvironments[Random.Range(0, beachEnvironments.Count)];
                    yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
                    {
                        EmteqManager.SetDataPoint($"Displaying Beach: {randomisedBeachIndex}");
                        EnvironmentManager.Instance.SetEnvironment(beachData, 0);
                    }); 
                    
                    yield return new WaitForSeconds(_environmentHoldDuration);
                    EmteqManager.SetDataPoint($"Scene End");
                }
            }
            
            // Return To Home Screen
            yield return TransitionManager.Instance.TransitionEnvironmentsCoroutine(() =>
            {
                EnvironmentManager.Instance.Return();
                _affectScaleUIGameObject.SetActive(false);
            });
            
                    
            DataManager.Instance.experimentData.experimentEndTime = DateTime.Now;
            EmteqManager.SetDataPoint($"Experiment End");

            DataManager.Instance.WriteToFile();
            _programStateMachine.ActiveState = _programStateMachine.programStateEnd;
        }
        
        private string ConvertScale(in Vector2 scaleValue) => scaleValue switch
        {
            { magnitude: < 1f / 9f } => ScoreRing_1(scaleValue),
            { magnitude: < 3f / 9f } => ScoreRing_2(scaleValue),
            { magnitude: < 5f / 9f } => ScoreRing_3(scaleValue),
            { magnitude: < 7f / 9f } => ScoreRing_4(scaleValue),
            _ => ScoreRing_5(scaleValue)
        };

        private string ScoreRing_1(in Vector2 scaleValue) => "5:5";
        
        private string ScoreRing_2(in Vector2 scaleValue)
        {
            float signedAngle = Vector2.SignedAngle(Vector2.up, scaleValue.normalized);
            float offsetSignedAngle = signedAngle + (45f / 2f);
            float moduloAngle = offsetSignedAngle % 360;
            return moduloAngle switch
            {
                <= 45 => "5:6",
                <= 90 => "6:6",
                <= 135 => "6:5",
                <= 180 => "6:4",
                <= 225 => "5:4",
                <= 270 => "4:4",
                <= 315 => "4:5",
                <= 360 => "4:6",
                _ => "undefined"
            };
        }

        private string ScoreRing_3(in Vector2 scaleValue)
        {
            float signedAngle = Vector2.SignedAngle(Vector2.up, scaleValue.normalized);
            float offsetSignedAngle = signedAngle + (22.5f / 2f);
            float moduloAngle = offsetSignedAngle % 360;
            return moduloAngle switch
            {
                <= 22.5f => "5:7",
                <= 45 => "6:7",
                <= 67.5f => "7:7",
                <= 90 => "7:6",
                <= 112.5f => "7:5",
                <= 135 => "7:4",
                <= 157.5f => "7:3",
                <= 180 => "6:3",
                <= 202.5f => "5:3",
                <= 225 => "4:3",
                <= 247.5f => "3:3",
                <= 270 => "3:4",
                <= 292.5f => "3:5",
                <= 315 => "3:6",
                <= 337.5f => "3:7",
                <= 360 => "4:7",
                _ => "undefined"
            };
        }

        private string ScoreRing_4(in Vector2 scaleValue)
        {
            float signedAngle = Vector2.SignedAngle(Vector2.up, scaleValue.normalized);
            float offsetSignedAngle = signedAngle + (15f / 2f);
            float moduloAngle = offsetSignedAngle % 360;
            return moduloAngle switch
            {
                <= 15f => "5:8",
                <= 30f => "6:8",
                <= 45f => "7:8",
                <= 60f => "8:8",
                <= 75f => "8:7",
                <= 90f => "8:6",
                <= 105f => "8:5",
                <= 120f => "8:4",
                <= 135f => "8:3",
                <= 150f => "8:2",
                <= 165f => "7:2",
                <= 180f => "6:2",
                <= 195f => "5:2",
                <= 210f => "4:2",
                <= 225f => "3:2",
                <= 240f => "2:2",
                <= 255f => "2:3",
                <= 270f => "2:4",
                <= 285f => "2:5",
                <= 300f => "2:6",
                <= 315f => "2:7",
                <= 330f => "2:8",
                <= 345f => "3:8",
                <= 360f => "4:8",
                _ => "undefined"
            };
        }

        private string ScoreRing_5(in Vector2 scaleValue)
        {
            float signedAngle = Vector2.SignedAngle(Vector2.up, scaleValue.normalized);
            float offsetSignedAngle = signedAngle + (11.25f / 2f);
            float moduloAngle = offsetSignedAngle % 360;
            return moduloAngle switch
            {
                <= 11.25f => "5:9",
                <= 22.5f => "6:9",
                <= 33.75f => "7:9",
                <= 45f => "8:9",
                <= 56.25f => "9:9",
                <= 67.5f => "9:8",
                <= 78.75f => "9:7",
                <= 90f => "9:6",
                <= 101.25f => "9:5",
                <= 112.5f => "9:4",
                <= 123.75f => "9:3",
                <= 135f => "9:2",
                <= 146.25f => "9:1",
                <= 157.5f => "8:1",
                <= 168.75f => "7:1",
                <= 180f => "6:1",
                <= 191.25f => "5:1",
                <= 202.5f => "4:1",
                <= 213.75f => "3:1",
                <= 225f => "2:1",
                <= 236.25f => "1:1",
                <= 247.5f => "1:2",
                <= 258.75f => "1:3",
                <= 270f => "1:4",
                <= 281.25f => "1:5",
                <= 292.5f => "1:6",
                <= 303.75f => "1:7",
                <= 315f => "1:8",
                <= 326.25f => "1:9",
                <= 337.5f => "2:9",
                <= 348.75f => "3:9",
                <= 360f => "4:9",
                _ => "undefined"
            };
        }
    }
}
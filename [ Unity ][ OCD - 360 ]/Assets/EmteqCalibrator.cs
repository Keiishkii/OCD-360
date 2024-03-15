using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EmteqCalibrator : MonoBehaviour
{
    #region [ Serialised Fields ]
    [SerializeField] private int _heartbeatRecordingDuration;
    [SerializeField] private int _expressionRecordingDuration;
    [SerializeField] private InputActionReference _triggerPressInputActionReference;
    [SerializeField] private List<EmotionRecordingDataScriptableObject> _emotionRecordingData = new List<EmotionRecordingDataScriptableObject>();
    #endregion

    #region [ Unserialised Fields ]
    private LargeScreenUI _largeScreenUI;
    private static readonly int RecordingProperty = Animator.StringToHash("Record");
    private static readonly int EndRecordingProperty = Animator.StringToHash("End Recording");

    #endregion

    
    
    private void Awake()
    {
        _largeScreenUI = FindObjectOfType<LargeScreenUI>();
    }

    public IEnumerator RecordHeartbeatBaseline()
    {
        _largeScreenUI.HeartbeatRecordingInstructionsPanelVisible = true;
        yield return new WaitUntil(() => _triggerPressInputActionReference.action.WasPressedThisFrame());
        yield return null;
        
        
        _largeScreenUI.HeartbeatRecordingInstructionsPanelVisible = false;
        _largeScreenUI.HeartbeatRadialTimerVisible = true;
        
        for (float timeElapsed = 0; timeElapsed < _heartbeatRecordingDuration; timeElapsed += Time.deltaTime)
        {
            float value = Mathf.InverseLerp(0, _heartbeatRecordingDuration, timeElapsed);
            _largeScreenUI.HeartbeatRadialValue = value;

            yield return null;
        }
        
        _largeScreenUI.HeartbeatRadialTimerVisible = false;
    }

    public IEnumerator RecordExpressions()
    {
        _largeScreenUI.ExpressionVisible = true;
        _largeScreenUI.ProgressionSliderVisible = true;
        _largeScreenUI.RecordingDisplayVisible = true;
        
        _largeScreenUI.ProgressionSliderValue = 0;
        
        for (int i = 0; i < _emotionRecordingData.Count; i++)
        {
            _largeScreenUI.TimerVisible = false;
            _largeScreenUI.ExpressionRecordingInstructionsPanelVisible = true;

            _largeScreenUI.DisplayExpression(_emotionRecordingData[i]);
            
            yield return new WaitUntil(() => _triggerPressInputActionReference.action.WasPressedThisFrame());
            yield return null;
            
            _largeScreenUI.ExpressionRecordingDisplayAnimator.SetTrigger(RecordingProperty);
            
            _largeScreenUI.ExpressionRecordingInstructionsPanelVisible = false;
            _largeScreenUI.TimerVisible = true;
            
            for (float timeElapsed = 0; timeElapsed < _expressionRecordingDuration; timeElapsed += Time.deltaTime)
            {
                float value = Mathf.InverseLerp(0, _expressionRecordingDuration, timeElapsed);
                float valueOverFullCalibration = Mathf.InverseLerp(0, _emotionRecordingData.Count, (i + value));

                int timerValue = _expressionRecordingDuration - ((int)timeElapsed);

                _largeScreenUI.TimerValue = $"{timerValue}";
                _largeScreenUI.HeartbeatRadialValue = value;
                _largeScreenUI.ProgressionSliderValue = valueOverFullCalibration;

                yield return null;
            }
            
            _largeScreenUI.ExpressionRecordingDisplayAnimator.SetTrigger(EndRecordingProperty);
        }
        
        _largeScreenUI.ExpressionRecordingInstructionsPanelVisible = false;
        _largeScreenUI.ProgressionSliderVisible = false;
        _largeScreenUI.RecordingDisplayVisible = false;
        _largeScreenUI.ExpressionVisible = false;
        _largeScreenUI.TimerVisible = false;
    }
}

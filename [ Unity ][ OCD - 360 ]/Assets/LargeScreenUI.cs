using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LargeScreenUI : MonoBehaviour
{
    #region [ Serialised Fields ]
    [SerializeField] private GameObject _heartbeatRecordingInstructionsPanel;
    [SerializeField] private GameObject _heartbeatRadialTimerPanel;
    [SerializeField] private GameObject _expressionRecordingInstructionsPanel;
    [SerializeField] private GameObject _expressionPanel;
    [SerializeField] private GameObject _timerPanel;
    [SerializeField] private GameObject _progressionSliderPanel;
    [SerializeField] private GameObject _recordingDisplay;
    
    [SerializeField] private TMP_Text _expressionInstructions;
    [SerializeField] private TMP_Text _expressionName;
    [SerializeField] private TMP_Text _timerText;

    [SerializeField] private Slider _progressionSlider;
    
    [SerializeField] private Image _heartbeatRadialTimerImage;
    [SerializeField] private Image _expressionImage;
    [SerializeField] private Image _backgroundImage;
    
    [SerializeField] private Color _displayHibernationColour;
    
    public Animator ExpressionRecordingDisplayAnimator => _expressionRecordingDisplayAnimator;
    [SerializeField] private Animator _expressionRecordingDisplayAnimator;
    #endregion

    #region [ Unserialised Fields ]
    private bool _displayActive = false;
    private IEnumerator _displayActivationCoroutine;
    public bool DisplayActive
    {
        get => _displayActive;
        set
        {
            _displayActive = value;
            if (_displayActivationCoroutine != null) StopCoroutine(_displayActivationCoroutine);
            StartCoroutine(_displayActivationCoroutine = BackgroundColourTransition(value));
        }
    }

    public bool HeartbeatRecordingInstructionsPanelVisible { set => _heartbeatRecordingInstructionsPanel.SetActive(value); }
    public bool HeartbeatRadialTimerVisible { set => _heartbeatRadialTimerPanel.SetActive(value); }
    public bool ExpressionRecordingInstructionsPanelVisible { set => _expressionRecordingInstructionsPanel.SetActive(value); }
    public bool ExpressionVisible { set => _expressionPanel.SetActive(value); }
    public bool ProgressionSliderVisible { set => _progressionSliderPanel.SetActive(value); }
    public bool TimerVisible { set => _timerPanel.SetActive(value); }
    public bool RecordingDisplayVisible { set => _recordingDisplay.SetActive(value); }
    
    public string TimerValue { set => _timerText.text = value; }
    public float ProgressionSliderValue { set => _progressionSlider.value = value; }

    private float _heartbeatRadialValue = 0;
    public float HeartbeatRadialValue
    {
        get => _heartbeatRadialValue;
        set
        {
            _heartbeatRadialValue = Mathf.Clamp01(_heartbeatRadialValue);
            _heartbeatRadialTimerImage.fillAmount = value;
        }
    }
    #endregion


    
    private void Start() => _backgroundImage.color = _displayHibernationColour;

    
    
    public void DisplayExpression(EmotionRecordingDataScriptableObject data)
    {
        _expressionImage.sprite = data.expressionImageSprite;
        _expressionInstructions.text = data.instructions;
        _expressionName.text = data.displayName;
    }
    
    private IEnumerator BackgroundColourTransition(bool active)
    {
        Color activeColour = _backgroundImage.color;
        Color targetColour = active ? Color.white : _displayHibernationColour;

        float transitionDuration = 0.5f;
        for (float timeElapsed = 0; timeElapsed < transitionDuration; timeElapsed += Time.deltaTime)
        {
            float value = Mathf.InverseLerp(0, transitionDuration, timeElapsed);
            _backgroundImage.color = Color.Lerp(activeColour, targetColour, value);
            yield return null;
        }
        
        _backgroundImage.color = targetColour;
    }
}

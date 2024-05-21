using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    #region [ Instance ]
    private static TransitionManager _instance;
    public static TransitionManager Instance => _instance ? _instance : _instance = FindObjectOfType<TransitionManager>();
    #endregion
    
    #region [ Serialised Fields ]
    [SerializeField] private ExperimentTransitionUI _experimentTransitionUI;
    [SerializeField] private AnimationCurve _animationCurve;

    [SerializeField] private TransitionType _transitionType;
    [SerializeField] private float _fadeTransitionDuration;
    [SerializeField] private float _transitionHoldDuration; 
    #endregion

    #region [ Unserialised Feilds ]
    private ReminderUI _reminderUI;
    private IEnumerator _transitionCoroutine;
    private enum TransitionType
    {
        Immediate,
        Fade
    }
    #endregion


    private void Awake()
    {
        _reminderUI = FindObjectOfType<ReminderUI>();
    }


    public IEnumerator TransitionEnvironmentsCoroutine(Action action)
    {
        yield return _transitionType switch
        {
            TransitionType.Immediate => ImmediateTransitionCoroutine(action),
            TransitionType.Fade => FadeTransitionCoroutine(action),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private IEnumerator FadeTransitionCoroutine(Action action)
    {
        _experimentTransitionUI.Interactable = true;
            
        #region [ Fade To Black ]
        for (float timeElapsed = 0; timeElapsed < _fadeTransitionDuration; timeElapsed += Time.deltaTime)
        {
            float weight = Mathf.InverseLerp(0, _fadeTransitionDuration, timeElapsed);
            float evaluation = _animationCurve.Evaluate(weight);
            _experimentTransitionUI.Alpha = evaluation;
                
            yield return null;
        }
        _experimentTransitionUI.Alpha = 1;
        #endregion


        _reminderUI.Visible = true;
        yield return new WaitForSeconds(_transitionHoldDuration);
        action.Invoke();
        _reminderUI.Visible = false;
            
        #region [ Fade From Black ]
        for (float timeElapsed = 0; timeElapsed < _fadeTransitionDuration; timeElapsed += Time.deltaTime)
        {
            float weight = 1 - Mathf.InverseLerp(0, _fadeTransitionDuration, timeElapsed);
            float evaluation = _animationCurve.Evaluate(weight);
            _experimentTransitionUI.Alpha = evaluation;
                
            yield return null;
        }
        _experimentTransitionUI.Alpha = 0;
        #endregion
            
        _experimentTransitionUI.Interactable = false;
    }

    private IEnumerator ImmediateTransitionCoroutine(Action action)
    {
        _experimentTransitionUI.Interactable = true;
        _experimentTransitionUI.Alpha = 1;

        _reminderUI.Visible = true;
        yield return new WaitForSeconds(_transitionHoldDuration);
        action.Invoke();
        _reminderUI.Visible = false;
            
        _experimentTransitionUI.Alpha = 0;
        _experimentTransitionUI.Interactable = false;
    }
}

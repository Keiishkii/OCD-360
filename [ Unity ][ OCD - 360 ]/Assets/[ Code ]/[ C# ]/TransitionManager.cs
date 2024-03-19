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
    [SerializeField] private float _transitionDuration;
    #endregion

    #region [ Unserialised Feilds ]
    private IEnumerator _transitionCoroutine;
    #endregion


    
    public IEnumerator TransitionEnvironmentsCoroutine(Action action)
    {
        _experimentTransitionUI.Interactable = true;
            
        #region [ Fade To Black ]
        for (float timeElapsed = 0; timeElapsed < _transitionDuration; timeElapsed += Time.deltaTime)
        {
            float weight = Mathf.InverseLerp(0, _transitionDuration, timeElapsed);
            float evaluation = _animationCurve.Evaluate(weight);
            _experimentTransitionUI.Alpha = evaluation;
                
            yield return null;
        }
        _experimentTransitionUI.Alpha = 1;
        #endregion

        action.Invoke();
            
        #region [ Fade From Black ]
        for (float timeElapsed = 0; timeElapsed < _transitionDuration; timeElapsed += Time.deltaTime)
        {
            float weight = 1 - Mathf.InverseLerp(0, _transitionDuration, timeElapsed);
            float evaluation = _animationCurve.Evaluate(weight);
            _experimentTransitionUI.Alpha = evaluation;
                
            yield return null;
        }
        _experimentTransitionUI.Alpha = 0;
        #endregion
            
        _experimentTransitionUI.Interactable = false;
    }
}

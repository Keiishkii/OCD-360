using UnityEngine;
using UnityEngine.UIElements;

public class ExperimentTransitionUI : MonoBehaviour
{
    #region [ Serialsed Fields ]
    [SerializeField] public CanvasGroup _canvasGroup;
    #endregion
    
    #region [ Unserialised Fields ]
    private float _alpha = 0f;
    public float Alpha
    {
        get => _alpha;
        set
        {
            _alpha = value;
            _canvasGroup.alpha = value;
        }
    }

    private bool _interactable = false;
    public bool Interactable
    {
        get => _interactable;
        set
        {
            _interactable = value;
            _canvasGroup.interactable = value;
        }
    }
    #endregion


    
    private void Awake()
    {
        Interactable = false;
        Alpha = 0;
    }
}

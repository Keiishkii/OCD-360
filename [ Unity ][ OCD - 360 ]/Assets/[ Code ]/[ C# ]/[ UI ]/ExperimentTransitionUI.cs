using UnityEngine;
using UnityEngine.UIElements;

public class ExperimentTransitionUI : MonoBehaviour
{
    #region [ Serialised Fields ]
    private float _alpha = 0f;
    public float Alpha
    {
        get => _alpha;
        set
        {
            _alpha = value;
            _backgroundVisualElement.style.backgroundColor = new StyleColor(new Color(0, 0, 0, value));
        }
    }

    private bool _interactable = false;
    public bool Interactable
    {
        get => _interactable;
        set
        {
            _interactable = value;
            _backgroundVisualElement.pickingMode = (value) ? PickingMode.Position : PickingMode.Ignore;
        }
    }
    #endregion
    
    #region [ Unserialised Fields ]
    private UIDocument _uiDocument;
    private VisualElement _root;
    
    private VisualElement _backgroundVisualElement;
    #endregion


    
    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;
        
        _backgroundVisualElement = _root.Query<VisualElement>("Background");

        Interactable = false;
        Alpha = 0;
    }
}

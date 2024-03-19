using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InstructionUI : MonoBehaviour
{
    #if UNITY_EDITOR
    #region [ Editor ]
    [CustomEditor(typeof(InstructionUI))]
    public class InstructionUIEditor : KeiishkiiLib.CustomInspector<InstructionUI>
    {
        private Vector3 _forward;
        
        public override void OnInspectorGUI()
        {
            KeiishkiiLib.InspectorUtility.VectorField("Editor_Forward", ref _forward);
            KeiishkiiLib.InspectorUtility.Button("Test Rotation", false, () => _targetScript.SetPanelPosition(_forward));
            
            base.OnInspectorGUI();
        }
    }
    #endregion
    #endif
    
    
    #region [ Serialised Fields ]
    [SerializeField] private TMP_Text _titleField;
    [SerializeField] private TMP_Text _contentField;
    [SerializeField] private TMP_Text _continueConditionField;
    #endregion

    #region [ Unserialised Feilds ]
    private Transform _transform;
    private Vector3 _basePosition;
    private Quaternion _baseRotation;

    private GameObject _titleFieldGameObject;
    private GameObject _contentFieldGameObject;
    private GameObject _continueConditionFieldGameObject;
    #endregion

    

    private void Awake()
    {
        _transform = transform;
        _basePosition = _transform.position;
        _baseRotation = _transform.rotation;

        _titleFieldGameObject = _titleField.gameObject;
        _contentFieldGameObject = _contentField.gameObject;
        _continueConditionFieldGameObject = _continueConditionField.gameObject;
        
        SetPanelPosition(Vector3.forward);
    }
    
    
    
    public void SetPanelContent(TutorialContentScriptableObject data)
    {
        _titleFieldGameObject.SetActive(data.displayTitle);
        _titleField.text = data.title;
        
        _contentFieldGameObject.SetActive(data.displayContent);
        _contentField.text = data.content;
        
        _continueConditionFieldGameObject.SetActive(data.displayContinueCondition);
        _continueConditionField.text = data.continueCondition;
    }
    
    public void SetPanelPosition(Vector3 forward)
    {
        Vector3 projectedForward = Vector3.ProjectOnPlane(forward, Vector3.up);
        Quaternion rotation = Quaternion.LookRotation(projectedForward);
        
        _transform.SetPositionAndRotation(rotation * _basePosition, rotation * _baseRotation);
    }
}

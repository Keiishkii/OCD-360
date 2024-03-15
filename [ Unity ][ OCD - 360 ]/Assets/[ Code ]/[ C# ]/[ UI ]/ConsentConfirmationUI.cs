using MINELab;
using UnityEngine;
using UnityEngine.UI;

public class ConsentConfirmationUI : MonoBehaviour
{
    #region [ Serialised Fields ]
    [SerializeField] private Button _startButton;
    #endregion
    
    #region [ Unserialised Fields ]
    private ProgramManager _programManager;
    #endregion


    private void Awake()
    {
        _programManager = FindObjectOfType<ProgramManager>();
    }

    private void OnEnable()
    {
        Debug.Log("Test");
        _startButton.onClick.AddListener(OnStartButtonPressed);
    }

    private void OnDisable() => _startButton.onClick.RemoveListener(OnStartButtonPressed);
    
    private void OnStartButtonPressed()
    {
        Debug.Log("Test");
        _programManager.programStateMachine.ActiveState = _programManager.programStateMachine.programStateCalibration;
    }
}

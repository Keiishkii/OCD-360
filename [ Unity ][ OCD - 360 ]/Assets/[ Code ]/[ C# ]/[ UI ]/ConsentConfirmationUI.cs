using System.Threading;
using MINELab;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ConsentConfirmationUI : MonoBehaviour
{
    #region [ Serialised Fields ]
    [SerializeField] private Button _startButton;
    [SerializeField] private InputActionReference _triggerPressInputActionReference;
    #endregion
    
    #region [ Unserialised Fields ]
    private ProgramManager _programManager;
    #endregion

    

    private void Awake() => _programManager = FindObjectOfType<ProgramManager>();

    private void OnEnable()
    {
        _startButton.onClick.AddListener(OnStartButtonPressed);
        _triggerPressInputActionReference.action.performed += OnStartButtonPressed;
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(OnStartButtonPressed);
        _triggerPressInputActionReference.action.performed -= OnStartButtonPressed;
    }

    private void OnStartButtonPressed(InputAction.CallbackContext callback) => OnStartButtonPressed();
    private void OnStartButtonPressed()
    {
        Debug.Log("Test");
        _programManager.programStateMachine.ActiveState = _programManager.programStateMachine.programStateCalibration;
    }
}

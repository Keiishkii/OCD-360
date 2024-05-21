using System.Threading;
using MINELab;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ConsentConfirmationUI : MonoBehaviour
{
    #region [ Serialised Fields ]
    [SerializeField] private GameObject _canvasGameObject;
    [SerializeField] private Button _startButton;
    #endregion
    
    #region [ Unserialised Fields ]
    public bool Visible
    {
        get => _visible;
        set
        {
            _canvasGameObject.SetActive(value);
            _visible = value;

            if (value)
            {
                _startButton.onClick.AddListener(OnStartButtonPressed);
                InputManager.Instance.triggerPress.action.performed += OnStartButtonPressed;
            }
            else
            {
                _startButton.onClick.RemoveListener(OnStartButtonPressed);
                InputManager.Instance.triggerPress.action.performed -= OnStartButtonPressed;
            }
        }
    }
    private bool _visible;
    private ProgramManager _programManager;
    #endregion

    

    private void Awake() => _programManager = FindObjectOfType<ProgramManager>();



    private void OnStartButtonPressed(InputAction.CallbackContext obj) => OnStartButtonPressed();
    private void OnStartButtonPressed() => _programManager.programStateMachine.ActiveState = _programManager.programStateMachine.programStateCalibration;
}

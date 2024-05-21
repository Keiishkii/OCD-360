using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MINELab
{
    public class InputManager : MonoBehaviour
    {
        #region [ Instance ]
        public static InputManager Instance => _instance ? _instance : _instance = FindObjectOfType<InputManager>();
        private static InputManager _instance;
        #endregion

        #region [ Editor Action References ]
        public InputActionReference ratingSkip;
        #endregion

        #region [ Action References ]
        public InputActionReference triggerPress;
        public InputActionReference touchPadPress;
        public InputActionReference rightJoystick;
        #endregion

        #region [ Unserialised Fields ]
        public UnityEvent<Vector2> rightJoystickValueChangedEvent = new UnityEvent<Vector2>();
        private Vector2 _rightJoystickValue = Vector2.zero;
        public Vector2 RightJoystickValue
        {
            get => _rightJoystickValue;
            set
            {
                _rightJoystickValue = value;
                rightJoystickValueChangedEvent.Invoke(value);
            }
        }
        #endregion


        
        public void OnEnable()
        {
            rightJoystick.action.performed += OnRightJoystickInputPerformed;
            rightJoystick.action.canceled += OnRightJoystickInputCancelled;
        }

        public void OnDisable()
        {
            rightJoystick.action.performed += OnRightJoystickInputPerformed;
            rightJoystick.action.canceled += OnRightJoystickInputCancelled;
        }
        
        

        private void OnRightJoystickInputPerformed(InputAction.CallbackContext context)
        {
            RightJoystickValue = context.ReadValue<Vector2>();
            Debug.Log($"Performed: {RightJoystickValue}");
        }
    
        private void OnRightJoystickInputCancelled(InputAction.CallbackContext context)
        {
            RightJoystickValue = Vector2.zero;
            Debug.Log($"Cancelled: {RightJoystickValue}");
        }

        public IEnumerator WaitForChoice(Action<InputChoice> callback)
        {
            InputChoice choice = InputChoice.None;
            yield return new WaitUntil(() =>
            {
                if (triggerPress.action.WasPressedThisFrame())
                {
                    choice = InputChoice.Trigger;
                    return true;
                }
                
                if (touchPadPress.action.WasPressedThisFrame())
                {
                    choice = InputChoice.TouchPad;
                    return true;
                }
                
                return false;
            });
            yield return null;
            callback(choice);
        }
        
        public IEnumerator WaitForTriggerPress()
        {
            yield return new WaitUntil(() => triggerPress.action.WasPressedThisFrame());
            yield return null;
        }
        
        public IEnumerator WaitForTouchpadPress()
        {
            yield return new WaitUntil(() => touchPadPress.action.WasPressedThisFrame());
            yield return null;
        }
    }
}
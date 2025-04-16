using System;
using RooseLabs.Generics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace RooseLabs.Input
{
    public class InputManager : Singleton<InputManager>
    {
        private GameInput m_playerInputActions;
        private InputDevice m_currentDevice;
        private float m_pointerMoveTime;
        private bool m_allInputDisabled;

        public GameInput.GameplayActions GameplayActions { get; private set; }
        public GameInput.MenusActions MenusActions { get; private set; }

        #region Event Actions
        public event Action MenuPauseEvent = delegate { };
        public event Action MenuUnpauseEvent = delegate { };
        #endregion

        protected override void Awake()
        {
            base.Awake();
            m_playerInputActions = new GameInput();
            GameplayActions = m_playerInputActions.Gameplay;
            MenusActions = m_playerInputActions.Menus;
        }

        private void OnEnable()
        {
            InputSystem.onEvent += OnInputEvent;
            GameplayActions.Pause.performed += OnPause;
            MenusActions.Unpause.performed += OnUnpause;
        }

        private void OnDisable()
        {
            InputSystem.onEvent -= OnInputEvent;
            GameplayActions.Pause.performed -= OnPause;
            MenusActions.Unpause.performed -= OnUnpause;
        }

        private void OnPause(InputAction.CallbackContext context)
        {
            MenuPauseEvent.Invoke();
        }

        private void OnUnpause(InputAction.CallbackContext context)
        {
            MenuUnpauseEvent.Invoke();
        }

        private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
        {
            if (eventPtr.type != StateEvent.Type && eventPtr.type != DeltaStateEvent.Type) return;

            InputControl control = null;
            bool hasButtonPress = false;
            foreach (var c in eventPtr.EnumerateControls(InputControlExtensions.Enumerate.IgnoreControlsInDefaultState, device, InputSystem.settings.defaultButtonPressPoint))
            {
                control ??= c;
                if (c is not ButtonControl) continue;
                hasButtonPress = true;
                break;
            }
            if (control == null) return;

            if (!hasButtonPress && m_currentDevice is not null && m_currentDevice is not Pointer && device is Pointer && m_pointerMoveTime < 1.5f)
            {
                // When a pointer device moves, we wait for a while before setting it as the current device to avoid
                // accidental movements.
                m_pointerMoveTime += Time.deltaTime;
            }
            else
            {
                // On any input event, including button presses and joystick movements, update the current device.
                CurrentDevice = device;
            }
        }

        public void EnableGameplayInput()
        {
            MenusActions.Disable();
            GameplayActions.Enable();
            m_allInputDisabled = false;
        }

        public void EnableMenuInput()
        {
            GameplayActions.Disable();
            MenusActions.Enable();
            m_allInputDisabled = false;
        }

        public void DisableAllInput()
        {
            GameplayActions.Disable();
            MenusActions.Disable();
            m_allInputDisabled = true;
            Cursor.visible = false;
        }

        public InputDevice CurrentDevice
        {
            get => m_currentDevice;
            private set
            {
                // When updating the current device, reset the pointer move time
                // and set the cursor visibility based on the device type.
                m_currentDevice = value;
                m_pointerMoveTime = 0f;
                Cursor.visible = !m_allInputDisabled && value is Keyboard or Pointer;
            }
        }

        public bool IsCurrentDeviceKeyboardAndMouse()
        {
            return m_currentDevice is Keyboard or Pointer;
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace RooseLabs.Input
{
    public class InputManager
    {
        private static InputManager s_instance;
        private static readonly object s_lock = new();

        public static InputManager Instance
        {
            get
            {
                lock (s_lock)
                {
                    if (s_instance != null) return s_instance;
                    s_instance = new InputManager();
                    return s_instance;
                }
            }
        }

        private readonly GameInput m_gameInput;
        private InputDevice m_currentDevice;
        private float m_pointerMoveTime;
        private bool m_allInputDisabled;

        public GameInput.MenusActions MenusActions { get; }
        public GameInput.GameplayActions GameplayActions { get; }

        #region Event Actions
        public event Action<InputDevice> InputDeviceChangedEvent = delegate { };
        public event Action MenuPauseEvent = delegate { };
        public event Action MenuUnpauseEvent = delegate { };
        #endregion

        private InputManager()
        {
            m_gameInput = new GameInput();
            MenusActions = m_gameInput.Menus;
            GameplayActions = m_gameInput.Gameplay;

            InputSystem.onEvent += OnInputEvent;

            GameplayActions.Pause.performed += OnPause;
            MenusActions.Unpause.performed += OnUnpause;

            Application.quitting += OnApplicationQuit;
        }

        private void OnApplicationQuit()
        {
            Application.quitting -= OnApplicationQuit;
            DisableAllInput();
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
                // accidental movements. We also prevent Unity from processing the event any further.
                m_pointerMoveTime += Time.unscaledDeltaTime;
                eventPtr.handled = true;
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
                if (m_currentDevice != value) InputDeviceChangedEvent.Invoke(value);
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

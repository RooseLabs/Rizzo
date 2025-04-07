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

        public GameInput.GameplayActions GameplayActions { get; private set; }
        public GameInput.UIActions UIActions { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            m_playerInputActions = new GameInput();
            GameplayActions = m_playerInputActions.Gameplay;
            UIActions = m_playerInputActions.UI;
        }

        private void OnEnable()
        {
            InputSystem.onEvent += OnInputEvent;
        }

        private void OnDisable()
        {
            InputSystem.onEvent -= OnInputEvent;
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

            if (!hasButtonPress && m_currentDevice is not Pointer && device is Pointer && m_pointerMoveTime < 1.5f)
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

        public InputDevice CurrentDevice
        {
            get => m_currentDevice;
            private set
            {
                m_currentDevice = value;
                m_pointerMoveTime = 0f;
            }
        }

        public bool IsCurrentDeviceKeyboardAndMouse()
        {
            return m_currentDevice is Keyboard or Pointer;
        }
    }
}
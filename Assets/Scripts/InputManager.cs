using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

public class InputManager : Singleton<InputManager>
{
    private InputSystem_Actions _inputActions;
    private InputSystem_Actions.PlayerActions _playerActions;
    private InputSystem_Actions.UIActions _uiActions;
    private InputDevice _currentDevice;
    private float _pointerMoveTime;

    public InputSystem_Actions.PlayerActions PlayerActions => _playerActions;
    public InputSystem_Actions.UIActions UIActions => _uiActions;

    protected override void Awake()
    {
        base.Awake();
        _inputActions = new InputSystem_Actions();
        _playerActions = _inputActions.Player;
        _uiActions = _inputActions.UI;
    }

    private void OnEnable()
    {
        InputSystem.onEvent += OnInputEvent;
        _uiActions.Enable();
        _playerActions.Enable();
    }

    private void OnDisable()
    {
        _playerActions.Disable();
        _uiActions.Disable();
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

        if (!hasButtonPress && _currentDevice is not Pointer && device is Pointer && _pointerMoveTime < 1.5f)
        {
            // When a pointer device moves, we wait for a while before setting it as the current device to avoid
            // accidental movements.
            _pointerMoveTime += Time.deltaTime;
        }
        else
        {
            // On any input event, including button presses and joystick movements, update the current device.
            CurrentDevice = device;
        }
    }

    public InputDevice CurrentDevice
    {
        get => _currentDevice;
        private set
        {
            _currentDevice = value;
            _pointerMoveTime = 0f;
        }
    }

    public bool IsCurrentDeviceKeyboardAndMouse()
    {
        return _currentDevice is Keyboard or Pointer;
    }
}
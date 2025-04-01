using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputManager : Singleton<InputManager>
{
    private InputSystem_Actions _inputActions;
    private InputSystem_Actions.PlayerActions _playerActions;
    private InputSystem_Actions.UIActions _uiActions;
    private InputDevice _currentDevice;
    private IDisposable _eventListener;

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
        _eventListener = InputSystem.onAnyButtonPress.Call(OnAnyButtonPress);
        _uiActions.Enable();
        _playerActions.Enable();
    }

    private void OnDisable()
    {
        _playerActions.Disable();
        _uiActions.Disable();
        _eventListener.Dispose();
    }

    private void OnAnyButtonPress(InputControl control)
    {
        _currentDevice = control.device;
    }

    public InputDevice GetCurrentDevice()
    {
        return _currentDevice;
    }

    public bool IsCurrentDeviceKeyboardAndMouse()
    {
        return _currentDevice is Keyboard or Pointer;
    }
}
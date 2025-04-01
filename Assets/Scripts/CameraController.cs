using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField][Range(0.1f, 1.0f)] public float followSmoothTime = 0.1f;
    [SerializeField][Range(1f, 5f)] private float lookRadius = 2f;
    [SerializeField][Range(1f, 5f)] private float lookDamping = 1f;

    private Vector3 _velocity = Vector3.zero;
    private Vector3 _lookOffset = Vector3.zero;
    private bool _isTargetSet;
    private bool _followTarget = true;

    private InputManager _inputManager;
    private InputSystem_Actions.PlayerActions _playerActions;

    public bool FollowTarget
    {
        get => _followTarget;
        set => _followTarget = value;
    }

    private void Awake()
    {
        _inputManager = InputManager.Instance;
        _playerActions = _inputManager.PlayerActions;
        _isTargetSet = target != null;
    }

    private void FixedUpdate()
    {
        if (_isTargetSet && _followTarget)
        {
            Vector3 targetPosition = target.position;
            targetPosition.z = transform.position.z;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition + _lookOffset, ref _velocity, followSmoothTime);
        }
    }

    private void LateUpdate()
    {
        if (_isTargetSet && _inputManager.IsCurrentDeviceKeyboardAndMouse())
        {
            Vector2 mouseScreenPos = _playerActions.Look.ReadValue<Vector2>();
            Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            Vector2 offset = mouseScreenPos - screenCenter;
            
            // Normalize to -1 to 1 range
            offset /= screenCenter;
            
            // Calculate desired look offset
            Vector3 desiredLookOffset = new Vector3(offset.x, offset.y, 0f) * lookRadius;
            
            // Apply exponential slowdown
            float t = desiredLookOffset.magnitude / lookRadius;
            float scale = 1f - Mathf.Exp(-t * 3f);
            desiredLookOffset *= scale;
            _lookOffset = Vector3.Lerp(_lookOffset, desiredLookOffset, lookDamping * Time.deltaTime);
        }
        else
        {
            // Reset look offset when not using mouse/keyboard
            _lookOffset = Vector3.Lerp(_lookOffset, Vector3.zero, lookDamping * Time.deltaTime);
        }
    }
}

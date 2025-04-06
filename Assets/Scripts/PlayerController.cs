using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Actor3D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.5f;

    private InputSystem_Actions.PlayerActions _playerActions;
    private Rigidbody2D _rb;
    private Actor3D _actor3D;

    private Vector2 _moveInput;
    private bool _canDodge = true;
    private bool _isDodging;

    private PlayerAnimationStateController _animationStateController;

    private void Awake()
    {
        _playerActions = InputManager.Instance.PlayerActions;
        _rb = GetComponent<Rigidbody2D>();
        _actor3D = GetComponent<Actor3D>();
        _animationStateController = GetComponentInChildren<PlayerAnimationStateController>();
    }

    private void Update()
    {
        _animationStateController.UpdateAnimation(_moveInput, _rb.linearVelocity);
    }

    private void FixedUpdate()
    {
        if (_isDodging) return;
        _rb.linearVelocity = _moveInput * moveSpeed;
        if (_moveInput.magnitude > 0f)
        {
            Vector2 direction = _rb.position + _moveInput;
            _actor3D.LookAt(direction);
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        float magnitude = Mathf.Clamp01(_moveInput.magnitude);
        _moveInput.y *= 0.5f;
        _moveInput = _moveInput.normalized * magnitude;
    }

    private void OnDodge(InputAction.CallbackContext context)
    {
        if (!_canDodge || _isDodging) return;
        _canDodge = false;
        StartCoroutine(DodgeRoll(_actor3D.GetFacingDirection()));
    }

    private IEnumerator DodgeRoll(Vector2 direction)
    {
        _isDodging = true;
        _animationStateController.PlayAnimation(PlayerAnimation.Dodge);

        float dodgeDuration = 15f / 30f;
        float dodgeSpeed = moveSpeed;

        _rb.linearVelocity = direction * dodgeSpeed / dodgeDuration;

        yield return new WaitForSeconds(dodgeDuration);

        _isDodging = false;
        _canDodge = true;
    }

    private void OnEnable()
    {
        _playerActions.Move.performed += OnMove;
        _playerActions.Move.canceled += OnMove;
        _playerActions.Dodge.performed += OnDodge;
    }

    private void OnDisable()
    {
        _playerActions.Move.performed -= OnMove;
        _playerActions.Move.canceled -= OnMove;
        _playerActions.Dodge.performed -= OnDodge;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Entity3D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.5f;

    private InputSystem_Actions.PlayerActions _playerActions;
    private Camera _cam;
    private Rigidbody2D _rb;
    private Entity3D _entity3D;
    private Vector2 _moveInput;
    private bool _isAttacking;
    private Animator _animator;
    private float _idleAnimationTimer = 0f; 

    private void Awake()
    {
        _playerActions = InputManager.Instance.PlayerActions;
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        _entity3D = GetComponent<Entity3D>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Look();
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _moveInput * moveSpeed;
    }

    private void LateUpdate()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        _animator.SetFloat("WalkSpeed", (_rb.linearVelocity / moveSpeed).magnitude);
        if (_rb.linearVelocity.magnitude == 0f)
        {
            _idleAnimationTimer -= Time.deltaTime;
            if (_idleAnimationTimer <= 0)
            {
                _animator.SetTrigger("Idle_2");
                _idleAnimationTimer = Random.Range(10f, 20f);
            }
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        float magnitude = Mathf.Clamp01(_moveInput.magnitude);
        _moveInput.y *= 0.5f;
        _moveInput = _moveInput.normalized * magnitude;
    }

    private void Look()
    {
        Vector2 pointerPos = _cam.ScreenToWorldPoint(_playerActions.Look.ReadValue<Vector2>());
        _entity3D.LookAt(pointerPos);
    }

    private void OnEnable() 
    {
        _playerActions.Move.performed += OnMove;
        _playerActions.Move.canceled += OnMove;
    }

    private void OnDisable() 
    {
        _playerActions.Move.performed -= OnMove;
        _playerActions.Move.canceled -= OnMove;
    }
}

using RooseLabs.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RooseLabs.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private GameInput.GameplayActions m_actions;

        public Vector2 MoveInput { get; private set; }
        public bool PressedDodge { get; private set; }
        public bool PressedPrimaryAttack { get; private set; }
        public bool IsPressingPrimaryAttack { get; private set; }
        public bool PressedSecondaryAttack { get; private set; }
        public bool IsPressingSecondaryAttack { get; private set; }

        private void Awake()
        {
            m_actions = InputManager.Instance.GameplayActions;
        }

        private void OnEnable()
        {
            m_actions.Move.performed += OnMove;
            m_actions.Move.canceled += OnMove;
            m_actions.Dodge.performed += OnDodge;
            m_actions.Dodge.canceled += OnDodge;
            m_actions.PrimaryAttack.performed += OnPrimaryAttack;
            m_actions.PrimaryAttack.canceled += OnPrimaryAttack;
            m_actions.SecondaryAttack.performed += OnSecondaryAttack;
            m_actions.SecondaryAttack.canceled += OnSecondaryAttack;
            InputManager.Instance.EnableGameplayInput();
        }

        private void OnDisable()
        {
            m_actions.Move.performed -= OnMove;
            m_actions.Move.canceled -= OnMove;
            m_actions.Dodge.performed -= OnDodge;
            m_actions.Dodge.canceled -= OnDodge;
            m_actions.PrimaryAttack.performed -= OnPrimaryAttack;
            m_actions.PrimaryAttack.canceled -= OnPrimaryAttack;
            m_actions.SecondaryAttack.performed -= OnSecondaryAttack;
            m_actions.SecondaryAttack.canceled -= OnSecondaryAttack;
        }

        private void LateUpdate()
        {
            PressedDodge = false;
            PressedPrimaryAttack = false;
            PressedSecondaryAttack = false;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            float magnitude = Mathf.Clamp01(moveInput.magnitude);
            moveInput.y *= 0.5f;
            moveInput = moveInput.normalized * magnitude;
            MoveInput = moveInput;
        }

        private void OnDodge(InputAction.CallbackContext context)
        {
            PressedDodge = context.performed;
        }

        private void OnPrimaryAttack(InputAction.CallbackContext context)
        {
            PressedPrimaryAttack = context.performed;
            IsPressingPrimaryAttack = context.performed;
        }

        private void OnSecondaryAttack(InputAction.CallbackContext context)
        {
            PressedSecondaryAttack = context.performed;
            IsPressingSecondaryAttack = context.performed;
        }
    }
}

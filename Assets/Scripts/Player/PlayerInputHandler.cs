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
            m_actions.Move.performed += OnMoveInput;
            m_actions.Move.canceled += OnMoveInput;
            m_actions.Dodge.performed += OnDodgeInput;
            m_actions.Dodge.canceled += OnDodgeInput;
            m_actions.PrimaryAttack.performed += OnPrimaryAttackInput;
            m_actions.PrimaryAttack.canceled += OnPrimaryAttackInput;
            m_actions.SecondaryAttack.performed += OnSecondaryAttackInput;
            m_actions.SecondaryAttack.canceled += OnSecondaryAttackInput;
            m_actions.Enable();
        }

        private void OnDisable()
        {
            m_actions.Disable();
            m_actions.Move.performed -= OnMoveInput;
            m_actions.Move.canceled -= OnMoveInput;
            m_actions.Dodge.performed -= OnDodgeInput;
            m_actions.Dodge.canceled -= OnDodgeInput;
            m_actions.PrimaryAttack.performed -= OnPrimaryAttackInput;
            m_actions.PrimaryAttack.canceled -= OnPrimaryAttackInput;
            m_actions.SecondaryAttack.performed -= OnSecondaryAttackInput;
            m_actions.SecondaryAttack.canceled -= OnSecondaryAttackInput;
        }

        private void LateUpdate()
        {
            PressedDodge = false;
            PressedPrimaryAttack = false;
            PressedSecondaryAttack = false;
        }

        private void OnMoveInput(InputAction.CallbackContext context)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            float magnitude = Mathf.Clamp01(moveInput.magnitude);
            moveInput.y *= 0.5f;
            moveInput = moveInput.normalized * magnitude;
            MoveInput = moveInput;
        }

        private void OnDodgeInput(InputAction.CallbackContext context)
        {
            PressedDodge = context.performed;
        }

        private void OnPrimaryAttackInput(InputAction.CallbackContext context)
        {
            PressedPrimaryAttack = context.performed;
            IsPressingPrimaryAttack = context.performed;
        }

        private void OnSecondaryAttackInput(InputAction.CallbackContext context)
        {
            PressedSecondaryAttack = context.performed;
            IsPressingSecondaryAttack = context.performed;
        }
    }
}

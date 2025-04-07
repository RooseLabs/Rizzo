using System.Collections;
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
            m_actions.Enable();
        }

        private void OnDisable()
        {
            m_actions.Disable();
            m_actions.Move.performed -= OnMoveInput;
            m_actions.Move.canceled -= OnMoveInput;
            m_actions.Dodge.performed -= OnDodgeInput;
            m_actions.Dodge.canceled -= OnDodgeInput;
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
            if (PressedDodge) StartCoroutine(ConsumeDodgeInput());
        }

        private IEnumerator ConsumeDodgeInput()
        {
            yield return null; // Waits for one frame
            PressedDodge = false;
        }
    }
}
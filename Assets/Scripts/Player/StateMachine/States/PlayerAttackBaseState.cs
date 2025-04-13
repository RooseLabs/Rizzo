using RooseLabs.Enums;
using RooseLabs.Input;
using RooseLabs.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RooseLabs.Player.StateMachine.States
{
    public abstract class PlayerAttackBaseState<T> : PlayerState where T : BaseWeaponSO
    {
        protected readonly T WeaponData;
        protected readonly GameObject WeaponGO;

        private readonly Camera m_cam;
        private readonly InputManager m_inputManager;
        private readonly InputAction m_lookAction;

        protected bool PressedAttack =>
            WeaponData.Type == WeaponType.Primary
                ? Player.InputHandler.PressedPrimaryAttack
                : Player.InputHandler.PressedSecondaryAttack;

        protected bool IsPressingAttack =>
            WeaponData.Type == WeaponType.Primary
                ? Player.InputHandler.IsPressingPrimaryAttack
                : Player.InputHandler.IsPressingSecondaryAttack;

        protected bool PressedOppositeAttack =>
            WeaponData.Type == WeaponType.Primary
                ? Player.InputHandler.PressedSecondaryAttack
                : Player.InputHandler.PressedPrimaryAttack;

        protected PlayerState OppositeAttackState =>
            WeaponData.Type == WeaponType.Primary
                ? Player.SecondaryAttackState
                : Player.PrimaryAttackState;

        protected PlayerAttackBaseState(
            Player player,
            PlayerStateMachine stateMachine,
            T weaponData,
            GameObject weaponGO
        ) : base(player, stateMachine)
        {
            WeaponData = weaponData;
            WeaponGO = weaponGO;
            m_cam = Camera.main;
            m_inputManager = InputManager.Instance;
            m_lookAction = InputManager.Instance.GameplayActions.Look;
        }

        public override void OnEnter()
        {
            Player.RB.linearVelocity = Vector2.zero;
            Animator.SetFloat(Player.AnimationStateController.F_Velocity, 0f);
            WeaponGO?.SetActive(true);
            UpdateDirection();
        }

        protected void UpdateDirection()
        {
            if (m_inputManager.IsCurrentDeviceKeyboardAndMouse())
            {
                Player.Actor3D.LookAt(m_cam.ScreenToWorldPoint(m_lookAction.ReadValue<Vector2>()));
            }
            else
            {
                Player.Actor3D.LookAt(Player.RB.position + Player.InputHandler.MoveInput);
            }
        }

        public abstract void ResetCombo();
    }
}

using RooseLabs.Input;
using RooseLabs.ScriptableObjects;
using RooseLabs.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RooseLabs.Player.StateMachine.States
{
    public abstract class PlayerAttackBaseState : PlayerState
    {
        private int m_currentAttackIndex;
        private float m_minComboTime;
        private float m_maxComboTime;
        private float m_attackAnimEndTime;

        private readonly Camera m_cam;
        private readonly InputManager m_inputManager;
        private readonly InputAction m_lookAction;

        protected abstract bool PressedAttack { get; }
        protected abstract BaseWeaponSO WeaponData { get; }
        protected abstract GameObject WeaponGO { get; }

        protected PlayerAttackBaseState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
        {
            m_cam = Camera.main;
            m_inputManager = InputManager.Instance;
            m_lookAction = InputManager.Instance.GameplayActions.Look;
        }

        public override void OnEnter()
        {
            Player.RB.linearVelocity = Vector2.zero;
            WeaponGO?.SetActive(true);
            UpdateDirection();
            if (Time.time <= m_maxComboTime)
            {
                // Can still combo
                m_currentAttackIndex = (m_currentAttackIndex + 1) % WeaponData.Attacks.Length;
            }
            else
            {
                m_currentAttackIndex = 0;
            }
            Animator.Play(CurrentStateData.hash);
            m_minComboTime = Time.time + CurrentAttackData.MinComboTime;
            m_maxComboTime = Time.time + CurrentAttackData.MaxComboTime;
            m_attackAnimEndTime = Time.time + CurrentStateData.length;
        }

        public override void Update()
        {
            if (Player.InputHandler.PressedDodge && Player.DodgeState.CheckIfCanDodge())
            {
                // Allow for dodge to "animation cancel" the attack - it can also cancel the attack if done too early.
                // This dodge will skip the attack "end" animation too, thus we need to hide the weapon here since the
                // animation event in the "end" animation that does so won't be called.
                if (WeaponData.DodgeResetsCombo) ResetCombo();
                WeaponGO?.SetActive(false);
                StateMachine.ChangeState(Player.DodgeState);
                return;
            }

            if (PressedAttack && Time.time >= m_minComboTime && Time.time <= m_maxComboTime)
            {
                UpdateDirection();
                m_currentAttackIndex = (m_currentAttackIndex + 1) % WeaponData.Attacks.Length;
                Animator.CrossFadeInFixedTime(CurrentStateData.hash, 0.25f);
                m_minComboTime = Time.time + CurrentAttackData.MinComboTime;
                m_maxComboTime = Time.time + CurrentAttackData.MaxComboTime;
                m_attackAnimEndTime = Time.time + CurrentStateData.length;
            }
            else if (Time.time >= m_attackAnimEndTime)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        private void UpdateDirection()
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

        public void ResetCombo()
        {
            m_currentAttackIndex = 0;
            m_minComboTime = 0f;
            m_maxComboTime = 0f;
        }

        protected WeaponAttackData CurrentAttackData => WeaponData.Attacks[m_currentAttackIndex];
        protected AnimationStateData CurrentStateData => CurrentAttackData.AnimationState;
    }
}

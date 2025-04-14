using RooseLabs.Models;
using RooseLabs.ScriptableObjects;
using UnityEngine;

namespace RooseLabs.Player.StateMachine.States
{
    public class PlayerRangedAttackState : PlayerWeaponAttackState<RangedWeaponSO>
    {
        private bool m_isPlayingAimAnimation;
        private float m_aimAnimationEndTime;
        private int m_currentAttackIndex;
        private float m_minComboTime;
        private float m_maxComboTime;
        private float m_attackAnimEndTime;
        private float m_cooldownEndTime;

        public PlayerRangedAttackState(
            Player player,
            PlayerStateMachine stateMachine,
            RangedWeaponSO weaponData,
            GameObject weaponGO
        ) : base(player, stateMachine, weaponData, weaponGO) { }

        public override void OnEnter()
        {
            base.OnEnter();
            Animator.Play(WeaponData.AimAnimationState.hash);
            m_isPlayingAimAnimation = true;
            m_aimAnimationEndTime = Time.time + WeaponData.AimAnimationState.length;
            m_currentAttackIndex = 0;
            m_minComboTime = m_aimAnimationEndTime + CurrentAttackData.MinComboTime;
            m_maxComboTime = m_aimAnimationEndTime + CurrentAttackData.MaxComboTime;
            m_attackAnimEndTime = m_aimAnimationEndTime + CurrentStateData.length;
        }

        public override void Update()
        {
            base.Update();
            if (Player.InputHandler.PressedDodge && Player.DodgeState.CanDodge())
            {
                // Allow for dodge to "animation cancel" the attack - it can also cancel the attack if done too early.
                // This dodge will skip the attack "end" animation too, thus we need to hide the weapon here since the
                // animation event in the "end" animation that does so won't be called.
                if (WeaponData.DodgeResetsCombo) ResetCombo();
                WeaponGO?.SetActive(false);
                StateMachine.ChangeState(Player.DodgeState);
                return;
            }

            if (m_isPlayingAimAnimation)
            {
                if (Time.time >= m_aimAnimationEndTime)
                {
                    m_isPlayingAimAnimation = false;
                    Animator.Play(CurrentStateData.hash);
                }
                return;
            }

            if (Time.time >= m_minComboTime && Time.time <= m_maxComboTime)
            {
                if (PressedAttack)
                {
                    UpdateDirection();
                    m_currentAttackIndex = (m_currentAttackIndex + 1) % WeaponData.Attacks.Length;
                    Animator.Play(CurrentStateData.hash);
                    m_minComboTime = Time.time + CurrentAttackData.MinComboTime;
                    m_maxComboTime = Time.time + CurrentAttackData.MaxComboTime;
                    m_attackAnimEndTime = Time.time + CurrentStateData.length;
                }
                else if (PressedOppositeAttack)
                {
                    // Allows chaining combos between primary and secondary attacks
                    // TODO: (Unsure if we actually want this)
                    StateMachine.ChangeState(OppositeAttackState);
                }
            }
            else if (Time.time >= m_attackAnimEndTime)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        public override void OnExit()
        {
            m_cooldownEndTime = Time.time + WeaponData.Cooldown;
        }

        public override void ResetCombo()
        {
            m_currentAttackIndex = 0;
            m_minComboTime = 0f;
            m_maxComboTime = 0f;
        }

        public override bool CanAttack()
        {
            if (Time.time < m_cooldownEndTime) return false;

            return true;
        }

        private WeaponAttackData CurrentAttackData => WeaponData.Attacks[m_currentAttackIndex];
        private AnimationStateData CurrentStateData => CurrentAttackData.AnimationState;
    }
}

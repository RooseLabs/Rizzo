using RooseLabs.Models;
using RooseLabs.ScriptableObjects;
using UnityEngine;

namespace RooseLabs.Player.StateMachine.States
{
    public class PlayerMeleeAttackState : PlayerWeaponAttackState<MeleeWeaponSO>
    {
        private int m_currentAttackIndex;
        private float m_minComboTime;
        private float m_maxComboTime;
        private float m_attackAnimEndTime;

        public PlayerMeleeAttackState(
            Player player,
            PlayerStateMachine stateMachine,
            MeleeWeaponSO weaponData,
            GameObject weaponGO
        ) : base(player, stateMachine, weaponData, weaponGO) { }

        public override void OnEnter()
        {
            base.OnEnter();
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

            if (Time.time >= m_minComboTime && Time.time <= m_maxComboTime)
            {
                if (PressedAttack)
                {
                    UpdateDirection();
                    m_currentAttackIndex = (m_currentAttackIndex + 1) % WeaponData.Attacks.Length;
                    Animator.CrossFadeInFixedTime(CurrentStateData.hash, 0.25f);
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

        public override void ResetCombo()
        {
            m_currentAttackIndex = 0;
            m_minComboTime = 0f;
            m_maxComboTime = 0f;
        }

        public override bool CanAttack() => true;

        private WeaponAttackData CurrentAttackData => WeaponData.Attacks[m_currentAttackIndex];
        private AnimationStateData CurrentStateData => CurrentAttackData.AnimationState;
    }
}

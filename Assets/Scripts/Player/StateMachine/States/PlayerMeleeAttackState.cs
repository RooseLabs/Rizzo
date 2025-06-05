using RooseLabs.Gameplay.Combat;
using RooseLabs.Models;
using RooseLabs.ScriptableObjects.Weapons;
using UnityEngine;

namespace RooseLabs.Player.StateMachine.States
{
    public class PlayerMeleeAttackState : PlayerWeaponAttackState<MeleeWeaponSO>
    {
        private int m_currentAttackIndex;
        private float m_minComboTime;
        private float m_maxComboTime;
        private float m_attackAnimEndTime;

        private readonly Hitbox m_weaponHitbox;
        private readonly bool m_weaponHitboxAvailable;

        public PlayerMeleeAttackState(
            Player player,
            PlayerStateMachine stateMachine,
            MeleeWeaponSO weaponData,
            GameObject weaponGO
        ) : base(player, stateMachine, weaponData, weaponGO)
        {
            var hitbox = Object.Instantiate(weaponData.HitboxPrefab, Player.transform);
            m_weaponHitbox = hitbox.GetComponent<Hitbox>();
            m_weaponHitboxAvailable = m_weaponHitbox != null;
        }

        ~PlayerMeleeAttackState()
        {
            if (m_weaponHitboxAvailable) Object.Destroy(m_weaponHitbox.gameObject);
        }

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
            PerformAttack();
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

            m_weaponHitbox.transform.position = WeaponGO.transform.position;

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
                    PerformAttack();
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

        protected override void PerformAttack()
        {
            if (!m_weaponHitboxAvailable) return;
            float finalAttackDamage = GetAttackDamage(CurrentAttackData.BaseDamage);
            m_weaponHitbox.Perform(finalAttackDamage, CurrentStateData.length);
        }

        public override void OnExit()
        {
            m_weaponHitbox?.EndPerformance();
        }

        public override void ResetCombo()
        {
            m_currentAttackIndex = 0;
            m_minComboTime = 0f;
            m_maxComboTime = 0f;
        }

        public override bool CanAttack() => true;

        protected override float GetAttackDamage(float baseDamage)
        {
            return base.GetAttackDamage(baseDamage);
        }

        private WeaponAttackData CurrentAttackData => WeaponData.Attacks[m_currentAttackIndex];
        private AnimationStateData CurrentStateData => CurrentAttackData.AnimationState;
    }
}

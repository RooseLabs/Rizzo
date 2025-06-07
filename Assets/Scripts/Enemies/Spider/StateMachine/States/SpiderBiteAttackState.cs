using System.Collections;
using UnityEngine;

namespace RooseLabs.Enemies.Spider.StateMachine.States
{
    public class SpiderBiteAttackState : SpiderState
    {
        private bool m_isAttacking = false;
        private bool m_attackPerformed = false;

        private const float AttackDuration = 0.5f;

        public SpiderBiteAttackState(Spider spider, SpiderStateMachine stateMachine) : base(spider, stateMachine) { }

        public override void OnEnter()
        {
            m_isAttacking = true;
            m_attackPerformed = false;
            Spider.Animator.Play(Spider.A_BiteAttack);
            Spider.StartCoroutine(PerformBiteAttack());
        }

        public override void Update()
        {
            if (!m_isAttacking)
            {
                StateMachine.ChangeState(Spider.ChaseState);
            }
        }

        private IEnumerator PerformBiteAttack()
        {
            yield return new WaitForSeconds(1f);
            if (!m_attackPerformed)
            {
                Spider.Hitbox.Perform(Spider.EnemyData.BiteAttackBaseDamage, 1.0f);
                m_attackPerformed = true;
            }
            yield return new WaitForSeconds(AttackDuration - 0.2f);
            m_isAttacking = false;
        }

        public override void OnExit()
        {
            Spider.Hitbox.EndPerformance();
            m_isAttacking = false;
            m_attackPerformed = false;
        }
    }
}

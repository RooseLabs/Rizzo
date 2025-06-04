using System.Collections;
using UnityEngine;

namespace RooseLabs.Enemies.Spider.StateMachine.States
{
    public class SpiderJumpAttackState : SpiderState
    {
        private Vector2 m_jumpStartPosition;
        private Vector2 m_jumpTargetPosition;
        private float m_jumpElapsedTime = 0f;
        private bool m_isJumping = false;
        private bool m_jumpMovementStarted = false;
        private bool m_jumpIsLanding = false;

        private const float JumpDuration    = 0.5f; // Duration of the jump in seconds
        private const float LandingOffset   = 0.5f; // A positive value will make the spider jump behind the player
        private const float MaxJumpDistance = 5.0f; // Maximum distance the spider can jump

        public SpiderJumpAttackState(Spider spider, SpiderStateMachine stateMachine) : base(spider, stateMachine) { }

        public override void OnEnter()
        {
            if (m_isJumping)
            {
                StateMachine.ChangeState(Spider.ChaseState);
                return;
            }

            m_isJumping = true;
            m_jumpMovementStarted = false;
            m_jumpIsLanding = false;

            Spider.Animator.Play(Spider.A_JumpAnticipation);
            Spider.StartCoroutine(StartJumpAfterAnticipation());
        }

        public override void Update()
        {
            if (m_isJumping && !m_jumpMovementStarted)
            {
                Spider.Actor3D.LookAt(Player.RB.position, true);
            }
        }

        public override void FixedUpdate()
        {
            if (!m_jumpMovementStarted) return;

            m_jumpElapsedTime += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(m_jumpElapsedTime / JumpDuration);
            Vector3 newPosition = Vector3.Lerp(m_jumpStartPosition, m_jumpTargetPosition, t);
            Spider.RB.MovePosition(newPosition);
            Spider.Actor3D.LookAt(m_jumpTargetPosition, true);

            if (t >= 0.5f && !m_jumpIsLanding)
            {
                m_jumpIsLanding = true;
                Spider.Animator.Play(Spider.A_JumpLand);
            }

            if (t >= 1f && m_jumpIsLanding)
            {
                m_isJumping = false;
                m_jumpMovementStarted = false;
                m_jumpIsLanding = false;

                Spider.Collider.enabled = true;
                StateMachine.ChangeState(Spider.ChaseState);
            }
        }

        private IEnumerator StartJumpAfterAnticipation()
        {
            yield return new WaitForSeconds(34f / 30f); // Wait for anticipation animation to finish
            Spider.Collider.enabled = false;

            m_jumpStartPosition = Spider.RB.position;

            // Direction to player
            Vector2 toPlayer = (Player.RB.position - m_jumpStartPosition).normalized;

            // Desired target position
            Vector2 desiredTarget = Player.RB.position + toPlayer * LandingOffset;

            // Clamp to max jump distance
            Vector2 targetDistance = desiredTarget - m_jumpStartPosition;
            if (targetDistance.magnitude > MaxJumpDistance)
            {
                targetDistance = targetDistance.normalized * MaxJumpDistance;
            }

            m_jumpTargetPosition = m_jumpStartPosition + targetDistance;
            m_jumpElapsedTime = 0f;
            m_jumpMovementStarted = true;
        }
    }
}

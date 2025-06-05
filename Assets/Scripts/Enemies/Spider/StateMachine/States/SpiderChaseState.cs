using UnityEngine;

namespace RooseLabs.Enemies.Spider.StateMachine.States
{
    public class SpiderChaseState : SpiderState
    {
        private const float LoseInterestDistance = 6.0f; // Distance to stop chasing the player
        private const float Offset               = 1.0f; // Offset to stop before reaching the player
        private const float MinAttackDistance    = 4.0f; // Distance to start attacking the player

        private float m_distToPlayer;

        public SpiderChaseState(Spider spider, SpiderStateMachine stateMachine) : base(spider, stateMachine) { }

        public override void OnEnter()
        {
            m_distToPlayer = Vector2.Distance(Spider.RB.position, Player.RB.position);
        }

        public override void Update()
        {
            m_distToPlayer = Vector2.Distance(Spider.RB.position, Player.RB.position);

            if (m_distToPlayer > LoseInterestDistance)
            {
                Spider.WanderPosition = Spider.RB.position;
                StateMachine.ChangeState(Spider.WanderState);
                return;
            }

            Spider.Actor3D.LookAt(Player.RB.position, true);

            if (m_distToPlayer <= MinAttackDistance && !IsObstacleBetweenSpiderAndPlayer())
            {
                Spider.TryAttack(); // Try attack every frame if in range and clear line of sight
            }
        }

        public override void FixedUpdate()
        {
            Vector2 toPlayer = Player.RB.position - Spider.RB.position;
            float moveDistance = Spider.Stats.movementVelocity * Time.fixedDeltaTime;

            Vector2 targetPos = Player.RB.position - toPlayer.normalized * Offset;
            Vector2 movement = targetPos - Spider.RB.position;

            Vector2 separation = Spider.GetSeparationVector();

            // If the spider is too close to the player or if the movement is negligible, only apply separation vector
            if (movement.magnitude <= 0.05f || toPlayer.magnitude <= Offset)
            {
                Spider.RB.MovePosition(Spider.RB.position + separation * (5.0f * Time.fixedDeltaTime));
                Spider.Animator.SetFloat(Spider.F_Velocity, 0f);
                return;
            }

            Vector2 moveDirection = (movement.normalized + separation).normalized;
            Vector2 finalMove = moveDirection * Mathf.Min(moveDistance, movement.magnitude);

            Spider.RB.MovePosition(Spider.RB.position + finalMove);
            Spider.Animator.SetFloat(Spider.F_Velocity, Spider.Stats.movementVelocity);
        }

        public override void OnExit()
        {
            Spider.Animator.SetFloat(Spider.F_Velocity, 0f);
        }
    }
}

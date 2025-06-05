using UnityEngine;

namespace RooseLabs.Enemies.Spider.StateMachine.States
{
    public class SpiderWanderState : SpiderState
    {
        private const float WanderDelay       = 1.5f;  // Time to wait before picking a new destination
        private const float WanderRadius      = 1.5f;  // Radius within which the spider will wander
        private const float MinWanderDistance = 0.75f; // Minimum distance to the new destination from the spider's current position
        private const float ChaseDistance     = 6.0f;  // Distance at which the spider will start chasing the player

        private Vector2 m_wanderDestination;
        private float m_waitTimer = 0f;
        private bool m_waiting = false;

        public SpiderWanderState(Spider spider, SpiderStateMachine stateMachine) : base(spider, stateMachine) { }

        public override void OnEnter()
        {
            PickNewDestination();
            m_waiting = false;
            m_waitTimer = 0f;
        }

        public override void Update()
        {
            float distToPlayer = Vector2.Distance(Spider.RB.position, Player.RB.position);
            if (distToPlayer <= ChaseDistance && !IsObstacleBetweenSpiderAndPlayer())
            {
                StateMachine.ChangeState(Spider.ChaseState);
                return;
            }
            if (m_waiting)
            {
                m_waitTimer += Time.deltaTime;
                if (m_waitTimer >= WanderDelay)
                {
                    m_waiting = false;
                    PickNewDestination();
                }
                Spider.Animator.SetFloat(Spider.F_Velocity, 0f);
            }
        }

        public override void FixedUpdate()
        {
            if (m_waiting) return;

            Vector2 toTarget = m_wanderDestination - (Vector2)Spider.transform.position;
            float distance = toTarget.magnitude;

            float moveDistance = Spider.Stats.movementVelocity * Time.fixedDeltaTime;
            if (distance < 0.01f)
            {
                m_waiting = true;
                m_waitTimer = 0f;
                Spider.Animator.SetFloat(Spider.F_Velocity, 0f);
                return;
            }

            Vector2 separation = Spider.GetSeparationVector();
            Vector2 movement = (toTarget.normalized + separation).normalized * Mathf.Min(moveDistance, distance);
            Spider.RB.MovePosition(Spider.RB.position + movement);
            Spider.Actor3D.LookAt(m_wanderDestination, true);
            Spider.Animator.SetFloat(Spider.F_Velocity, Spider.Stats.movementVelocity);
        }

        private void PickNewDestination()
        {
            Vector2 randomPointInCircle = Random.insideUnitCircle * WanderRadius;
            Vector2 candidate = Spider.WanderPosition + randomPointInCircle;
            Vector2 distanceToCandidate = candidate - Spider.RB.position;
            if (distanceToCandidate.magnitude < MinWanderDistance)
            {
                PickNewDestination();
                return;
            }
            Vector2 directionToCandidate = distanceToCandidate.normalized;
            RaycastHit2D hit = Physics2D.Linecast(Spider.RB.position + directionToCandidate, candidate, Spider.ObstacleLayerMask);
            if (hit.collider != null)
            {
                Vector2 distanceToHit = hit.point - Spider.RB.position;
                if (distanceToHit.magnitude < MinWanderDistance)
                {
                    PickNewDestination();
                    return;
                }
                candidate = hit.point - distanceToHit.normalized * 0.5f;
            }
            m_wanderDestination = candidate;
        }
    }
}

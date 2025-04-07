using UnityEngine;

namespace RooseLabs.Player.StateMachine.States
{
    public class PlayerIdleState : PlayerState
    {
        private float m_idleTimer;
        private float m_idleAnimationTimer;

        public PlayerIdleState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        public override void OnEnter()
        {
            Player.RB.linearVelocity = Vector2.zero;
            Player.AnimationStateController.SetFloat(Player.AnimationStateController.F_Velocity, 0f);
            m_idleAnimationTimer = Random.Range(10f, 20f);
            m_idleTimer = 0f;
        }

        public override void Update()
        {
            if (Player.InputHandler.MoveInput.magnitude > 0f)
            {
                StateMachine.ChangeState(Player.MoveState);
            }
            else
            {
                m_idleTimer += Time.deltaTime;
                if (m_idleTimer >= m_idleAnimationTimer)
                {
                    Player.AnimationStateController.SetTrigger(Player.AnimationStateController.T_IdleGroom);
                    m_idleTimer = -139f / 30f; // Compensate for the animation length (IdleGroom is 139 frames at 30 FPS)
                    m_idleAnimationTimer = Random.Range(10f, 20f);
                }
            }
        }
    }
}
using UnityEngine;

namespace RooseLabs.Player.StateMachine.States
{
    public class PlayerDodgeState : PlayerState
    {
        private bool m_isDodging;
        private bool m_canDodge = true;
        private float m_lastDodgeTime;
        private float m_dodgeDuration;
        private Vector2 m_dodgeDirection;

        public bool IsDodging => m_isDodging;

        public PlayerDodgeState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        public override void OnEnter()
        {
            m_canDodge = false;
            m_isDodging = true;
            if (Player.InputHandler.MoveInput.magnitude > 0f)
            {
                Player.Actor3D.LookAt(Player.RB.position + Player.InputHandler.MoveInput);
            }
            m_dodgeDirection = Player.Actor3D.GetFacingDirection();
            m_dodgeDuration = 15f / 30f;
            Animator.Play(Player.AnimationStateController.A_Dodge);
            Player.RB.linearVelocity = m_dodgeDirection * Player.DodgeVelocity / m_dodgeDuration;
        }

        public override void Update()
        {
            m_dodgeDuration -= Time.deltaTime;
            if (m_dodgeDuration <= 0f)
            {
                m_isDodging = false;
                m_canDodge = true;
                m_lastDodgeTime = Time.time;
                if (Player.InputHandler.MoveInput.magnitude > 0f)
                {
                    StateMachine.ChangeState(Player.MoveState);
                }
                else
                {
                    StateMachine.ChangeState(Player.IdleState);
                }
            }
        }

        public bool CanDodge() {
            return m_canDodge && Time.time >= m_lastDodgeTime + Player.DodgeCooldown;
        }
    }
}

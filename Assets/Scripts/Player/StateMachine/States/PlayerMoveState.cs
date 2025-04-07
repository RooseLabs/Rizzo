using UnityEngine;

namespace RooseLabs.Player.StateMachine.States
{
    public class PlayerMoveState : PlayerState
    {
        public PlayerMoveState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        public override void Update()
        {
            if (Player.InputHandler.PressedDodge && Player.DodgeState.CheckIfCanDash())
            {
                StateMachine.ChangeState(Player.DodgeState);
                return;
            }

            float currentVelocity = Player.RB.linearVelocity.magnitude == 0f ? 0f : Player.RB.linearVelocity.magnitude;
            Player.AnimationStateController.SetFloat(Player.AnimationStateController.F_Velocity, currentVelocity);
        }

        public override void FixedUpdate()
        {
            Player.RB.linearVelocity = Player.InputHandler.MoveInput * Player.MovementVelocity;
            if (Player.InputHandler.MoveInput.magnitude > 0f)
            {
                Vector2 direction = Player.RB.position + Player.InputHandler.MoveInput;
                Player.Actor3D.LookAt(direction);
            }
            else
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }
    }
}
using UnityEngine;

namespace RooseLabs.Player.StateMachine.States
{
    public class PlayerDeathState : PlayerState
    {
        public PlayerDeathState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        public override void OnEnter()
        {
            Player.RB.linearVelocity = Vector2.zero;
            Animator.SetFloat(Player.AnimationStateController.F_Velocity, 0f);
        }
    }
}

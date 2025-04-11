using UnityEngine;

namespace RooseLabs.Player.StateMachine
{
    public class PlayerState : RooseLabs.StateMachine.BaseState
    {
        protected readonly Player Player;
        protected readonly PlayerStateMachine StateMachine;

        protected PlayerState(Player player, PlayerStateMachine stateMachine)
        {
            Player = player;
            StateMachine = stateMachine;
        }

        protected Animator Animator => Player.AnimationStateController.Animator;
    }
}

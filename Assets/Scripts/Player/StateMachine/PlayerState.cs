using RooseLabs.StateMachine;
using UnityEngine;

namespace RooseLabs.Player.StateMachine
{
    public class PlayerState : BaseState
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

using UnityEngine;

namespace RooseLabs.Player.StateMachine
{
    public class PlayerStateMachine : RooseLabs.StateMachine.StateMachine<PlayerState>
    {
        protected override void OnEnter()
        {
            // Debug.Log($"[PlayerStateMachine] Entering state: {CurrentState.GetType().Name}");
            base.OnEnter();
        }

        protected override void OnExit()
        {
            // Debug.Log($"[PlayerStateMachine] Exiting state: {CurrentState.GetType().Name}");
            base.OnExit();
        }
    }
}
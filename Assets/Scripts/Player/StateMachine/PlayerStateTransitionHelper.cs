namespace RooseLabs.Player.StateMachine
{
    public static class PlayerStateTransitionHelper
    {
        /// <summary>Handles common transitions from idle and move states to other states.</summary>
        /// <param name="player">The Player instance.</param>
        /// <param name="stateMachine">The Player State Machine instance.</param>
        /// <returns>True if a state transition occurred, False otherwise.</returns>
        public static bool HandleIdleAndMoveTransitions(Player player, PlayerStateMachine stateMachine)
        {
            if (player.InputHandler.PressedDodge && player.DodgeState.CanDodge())
            {
                stateMachine.ChangeState(player.DodgeState);
                return true;
            }

            if (player.InputHandler.PressedPrimaryAttack && player.PrimaryAttackState.CanAttack())
            {
                stateMachine.ChangeState(player.PrimaryAttackState);
                return true;
            }
            if (player.InputHandler.PressedSecondaryAttack && player.SecondaryAttackState.CanAttack())
            {
                stateMachine.ChangeState(player.SecondaryAttackState);
                return true;
            }

            return false;
        }
    }
}

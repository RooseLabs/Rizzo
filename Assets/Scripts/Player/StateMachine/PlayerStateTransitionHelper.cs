namespace RooseLabs.Player.StateMachine
{
    public static class PlayerStateTransitionHelper
    {
        /// <summary>
        ///   <para>Handles common transitions from idle and move states to other states.</para>
        ///   <para>Returns true if a transition occurred, false otherwise.</para>
        /// </summary>
        /// <param name="player">The Player instance.</param>
        /// <param name="stateMachine">The Player State Machine instance.</param>
        public static bool HandleIdleAndMoveTransitions(Player player, PlayerStateMachine stateMachine)
        {
            if (player.InputHandler.PressedDodge && player.DodgeState.CheckIfCanDodge())
            {
                stateMachine.ChangeState(player.DodgeState);
                return true;
            }

            if (player.InputHandler.PressedPrimaryAttack)
            {
                stateMachine.ChangeState(player.PrimaryAttackState);
                return true;
            }
            if (player.InputHandler.PressedSecondaryAttack)
            {
                stateMachine.ChangeState(player.SecondaryAttackState);
                return true;
            }

            return false;
        }
    }
}

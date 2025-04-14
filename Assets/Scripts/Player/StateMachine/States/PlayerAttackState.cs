namespace RooseLabs.Player.StateMachine.States
{
    public abstract class PlayerAttackState : PlayerState
    {
        protected PlayerAttackState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        public abstract bool CanAttack();
    }
}

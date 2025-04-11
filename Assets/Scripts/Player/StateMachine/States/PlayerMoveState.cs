namespace RooseLabs.Player.StateMachine.States
{
    public class PlayerMoveState : PlayerState
    {
        public PlayerMoveState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        public override void Update()
        {
            if (PlayerStateTransitionHelper.HandleIdleAndMoveTransitions(Player, StateMachine)) return;

            float currentVelocity = Player.RB.linearVelocity.magnitude == 0f ? 0f : Player.InputHandler.MoveInput.magnitude;
            Animator.SetFloat(Player.AnimationStateController.F_Velocity, currentVelocity);
        }

        public override void FixedUpdate()
        {
            Player.RB.linearVelocity = Player.InputHandler.MoveInput * Player.MovementVelocity;
            if (Player.InputHandler.MoveInput.magnitude > 0f)
            {
                Player.Actor3D.LookAt(Player.RB.position + Player.InputHandler.MoveInput, true);
            }
            else
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }
    }
}

using UnityEngine;

namespace RooseLabs.Enemies.Spider.StateMachine.States
{
    public class SpiderDeathState : SpiderState
    {
        public SpiderDeathState(Spider spider, SpiderStateMachine stateMachine) : base(spider, stateMachine) { }

        public override void OnEnter()
        {
            Spider.Animator.SetFloat(Spider.F_Velocity, 0f);
            Spider.Animator.Play(Spider.A_Death);
            Spider.RB.linearVelocity = Vector2.zero;
            Spider.Collider.enabled = false;
        }
    }
}

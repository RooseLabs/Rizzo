using UnityEngine;

namespace RooseLabs.Enemies.Spider.StateMachine.States
{
    public class SpiderSpawnState : SpiderState
    {
        private float m_timer = 0f;
        private const float WaitDuration = 45f / 30f;

        public SpiderSpawnState(Spider spider, SpiderStateMachine stateMachine) : base(spider, stateMachine) { }

        public override void Update()
        {
            m_timer += Time.deltaTime;
            if (m_timer >= WaitDuration)
            {
                StateMachine.ChangeState(Spider.WanderState);
            }
        }
    }
}

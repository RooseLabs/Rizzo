using RooseLabs.Gameplay;
using RooseLabs.StateMachine;

namespace RooseLabs.Enemies.Spider.StateMachine
{
    public class SpiderState : BaseState
    {
        protected readonly Spider Spider;
        protected readonly SpiderStateMachine StateMachine;
        protected readonly Player.Player Player;

        protected SpiderState(Spider spider, SpiderStateMachine stateMachine)
        {
            Spider = spider;
            StateMachine = stateMachine;
            Player = GameManager.Instance.Player;
        }
    }
}

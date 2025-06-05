using RooseLabs.Gameplay;
using RooseLabs.StateMachine;
using UnityEngine;

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

        protected bool IsObstacleBetweenSpiderAndPlayer()
        {
            RaycastHit2D hit = Physics2D.Linecast(Spider.RB.position, Player.RB.position, Spider.ObstacleLayerMask);
            return hit.collider != null;
        }
    }
}

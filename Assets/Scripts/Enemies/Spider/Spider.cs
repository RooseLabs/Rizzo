using RooseLabs.Enemies.Spider.StateMachine;
using RooseLabs.Enemies.Spider.StateMachine.States;
using RooseLabs.Enums;
using RooseLabs.Gameplay;
using RooseLabs.ScriptableObjects.Enemies;
using RooseLabs.ScriptableObjects.StatusEffects;
using UnityEngine;

namespace RooseLabs.Enemies.Spider
{
    public class Spider : Enemy<SpiderSO>
    {
        [SerializeField] private SpiderAilment ailment;

        #region State Machine
        private SpiderStateMachine StateMachine { get; set; }
        public SpiderSpawnState SpawnState { get; private set; }
        public SpiderWanderState WanderState { get; private set; }
        public SpiderChaseState ChaseState { get; private set; }
        public SpiderJumpAttackState JumpAttackState { get; private set; }
        #endregion

        #region Animator | Float Parameters
        public readonly int F_Velocity = Animator.StringToHash("Velocity");
        #endregion
        #region Animator | Animation States
        public readonly int A_JumpAnticipation = Animator.StringToHash("Attack_Jump_Anticipation");
        public readonly int A_JumpLand = Animator.StringToHash("Attack_Jump_Land");
        #endregion

        public Vector2 WanderPosition { get; set; }

        private SpiderAilmentSO m_ailmentData;

        private const float JumpMaxDistance  = 5.0f;
        private const float JumpMinDistance  = 3.5f;
        private const float SpitMaxDistance  = 3.0f;
        private const float SpitMinDistance  = 2.0f;
        private const float BiteMaxDistance  = 1.0f;

        private const float JumpCooldown  = 3.0f;
        private const float SpitCooldown  = 2.0f;
        private const float BiteCooldown  = 1.0f;
        private float m_jumpCooldownTimer = 0.0f;
        private float m_spitCooldownTimer = 0.0f;
        private float m_biteCooldownTimer = 0.0f;

        private Player.Player m_player;

        protected override void Awake()
        {
            base.Awake();

            StateMachine = new SpiderStateMachine();
            SpawnState = new SpiderSpawnState(this, StateMachine);
            WanderState = new SpiderWanderState(this, StateMachine);
            ChaseState = new SpiderChaseState(this, StateMachine);
            JumpAttackState = new SpiderJumpAttackState(this, StateMachine);

            WanderPosition = transform.position;
            m_player = GameManager.Instance.Player;
        }

        protected override void Initialize(SpiderSO enemyData)
        {
            base.Initialize(enemyData);
            m_ailmentData = enemyData.Ailment;

            StateMachine.Initialize(SpawnState);
        }

        private void Update()
        {
            if (m_jumpCooldownTimer > 0) m_jumpCooldownTimer -= Time.deltaTime;
            if (m_spitCooldownTimer > 0) m_spitCooldownTimer -= Time.deltaTime;
            if (m_biteCooldownTimer > 0) m_biteCooldownTimer -= Time.deltaTime;
            StateMachine.Update();
        }

        protected void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        public void TryAttack()
        {
            float dist = Vector2.Distance(RB.position, m_player.RB.position);

            if (dist is <= JumpMaxDistance and >= JumpMinDistance && m_jumpCooldownTimer <= 0)
            {
                m_jumpCooldownTimer = JumpCooldown;
                StateMachine.ChangeState(JumpAttackState);
            }
            else if (dist is <= SpitMaxDistance and >= SpitMinDistance && m_spitCooldownTimer <= 0)
            {
                // TODO: Spit logic
                m_spitCooldownTimer = SpitCooldown;
            }
            else if (dist <= BiteMaxDistance && m_biteCooldownTimer <= 0)
            {
                // TODO: Bite logic
                m_biteCooldownTimer = BiteCooldown;
            }
        }
    }
}

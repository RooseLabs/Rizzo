using RooseLabs.Player.StateMachine;
using RooseLabs.Player.StateMachine.States;
using UnityEngine;

namespace RooseLabs.Player
{
    [RequireComponent(typeof(Actor3D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class Player : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float movementVelocity = 2.5f;

        [Header("Dodge")]
        [SerializeField] private float dodgeVelocity = 2.5f;
        [SerializeField] private float dodgeCooldown = 0.5f;

        public Rigidbody2D RB { get; private set; }
        public Actor3D Actor3D { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        public PlayerAnimationStateController AnimationStateController { get; private set; }
        public PlayerStateMachine StateMachine { get; private set; }

        // States
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerDodgeState DodgeState { get; private set; }

        private void Awake()
        {
            RB = GetComponent<Rigidbody2D>();
            Actor3D = GetComponent<Actor3D>();
            InputHandler = GetComponent<PlayerInputHandler>();
            AnimationStateController = GetComponentInChildren<PlayerAnimationStateController>();

            StateMachine = new PlayerStateMachine();
            IdleState = new PlayerIdleState(this, StateMachine);
            MoveState = new PlayerMoveState(this, StateMachine);
            DodgeState = new PlayerDodgeState(this, StateMachine);
        }

        private void Start()
        {
            StateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            StateMachine.Update();
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        public float MovementVelocity => movementVelocity;

        public float DodgeVelocity => dodgeVelocity;

        public float DodgeCooldown => dodgeCooldown;
    }
}

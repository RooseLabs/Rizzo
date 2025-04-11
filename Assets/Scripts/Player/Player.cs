using RooseLabs.Enums;
using RooseLabs.Player.StateMachine;
using RooseLabs.Player.StateMachine.States;
using RooseLabs.ScriptableObjects;
using UnityEngine;

namespace RooseLabs.Player
{
    [RequireComponent(typeof(Actor3D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class Player : MonoBehaviour
    {
        #region Inspector Variables
        [Header("Movement")]
        [SerializeField] private float movementVelocity = 2.5f;

        [Header("Dodge")]
        [SerializeField] private float dodgeVelocity = 2.5f;
        [SerializeField] private float dodgeCooldown = 0.5f;

        [Header("Default Weapons")]
        [SerializeField] private BaseWeaponSO defaultPrimaryWeapon;
        [SerializeField] private BaseWeaponSO defaultSecondaryWeapon;

        [Header("Hand Socket Bones")]
        [SerializeField] private Transform leftHandSocket;
        [SerializeField] private Transform rightHandSocket;
        #endregion

        #region Components
        public Rigidbody2D RB { get; private set; }
        public Actor3D Actor3D { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        public PlayerAnimationStateController AnimationStateController { get; private set; }
        private PlayerStateMachine StateMachine { get; set; }
        #endregion

        #region States
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerDodgeState DodgeState { get; private set; }
        public PlayerPrimaryAttackState PrimaryAttackState { get; private set; }
        public PlayerSecondaryAttackState SecondaryAttackState { get; private set; }
        #endregion

        #region Weapons
        public BaseWeaponSO PrimaryWeaponSO { get; private set; }
        public BaseWeaponSO SecondaryWeaponSO { get; private set; }
        public GameObject PrimaryWeaponGO { get; private set; }
        public GameObject SecondaryWeaponGO { get; private set; }
        #endregion

        private void Awake()
        {
            RB = GetComponent<Rigidbody2D>();
            Actor3D = GetComponent<Actor3D>();
            InputHandler = GetComponent<PlayerInputHandler>();
            AnimationStateController = GetComponentInChildren<PlayerAnimationStateController>();
            AnimationStateController.OnHideWeaponsRequested += HideWeapons;

            StateMachine = new PlayerStateMachine();
            IdleState = new PlayerIdleState(this, StateMachine);
            MoveState = new PlayerMoveState(this, StateMachine);
            DodgeState = new PlayerDodgeState(this, StateMachine);
            PrimaryAttackState = new PlayerPrimaryAttackState(this, StateMachine);
            SecondaryAttackState = new PlayerSecondaryAttackState(this, StateMachine);

            PrimaryWeaponSO = defaultPrimaryWeapon;
            SecondaryWeaponSO = defaultSecondaryWeapon;
        }

        private void Start()
        {
            StateMachine.Initialize(IdleState);
            if (PrimaryWeaponSO.WeaponPrefab)
                PrimaryWeaponGO = Instantiate(PrimaryWeaponSO.WeaponPrefab, GetHandSocketTransform(PrimaryWeaponSO));
            if (SecondaryWeaponSO.WeaponPrefab)
                SecondaryWeaponGO = Instantiate(SecondaryWeaponSO.WeaponPrefab, GetHandSocketTransform(SecondaryWeaponSO));
            HideWeapons();
        }

        private void Update()
        {
            StateMachine.Update();
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        private Transform GetHandSocketTransform(BaseWeaponSO weapon)
        {
            return weapon.SocketHand == HandSocket.Left ? leftHandSocket : rightHandSocket;
        }

        private void HideWeapons()
        {
            PrimaryWeaponGO?.SetActive(false);
            SecondaryWeaponGO?.SetActive(false);
        }

        public float MovementVelocity => movementVelocity;

        public float DodgeVelocity => dodgeVelocity;

        public float DodgeCooldown => dodgeCooldown;
    }
}

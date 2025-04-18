using System;
using RooseLabs.Enums;
using RooseLabs.Models;
using RooseLabs.Player.StateMachine;
using RooseLabs.Player.StateMachine.States;
using RooseLabs.ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace RooseLabs.Player
{
    [RequireComponent(typeof(Actor3D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class Player : MonoBehaviour
    {
        private PlayerStats m_stats;
        private LayerMask m_actorsMask;

        #region Inspector Variables
        [SerializeField] private PlayerStatsSO baseStats;

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
        public PlayerAttackState PrimaryAttackState { get; private set; }
        public PlayerAttackState SecondaryAttackState { get; private set; }
        #endregion

        #region Weapons
        private BaseWeaponSO PrimaryWeaponSO { get; set; }
        private BaseWeaponSO SecondaryWeaponSO { get; set; }
        private GameObject PrimaryWeaponGO { get; set; }
        private GameObject SecondaryWeaponGO { get; set; }
        #endregion

        private void Awake()
        {
            m_actorsMask = LayerMask.GetMask("Actors");

            RB = GetComponent<Rigidbody2D>();
            Actor3D = GetComponent<Actor3D>();
            InputHandler = GetComponent<PlayerInputHandler>();
            AnimationStateController = GetComponentInChildren<PlayerAnimationStateController>();
            AnimationStateController.OnHideWeaponsRequested += HideWeapons;

            StateMachine = new PlayerStateMachine();
            IdleState = new PlayerIdleState(this, StateMachine);
            MoveState = new PlayerMoveState(this, StateMachine);
            DodgeState = new PlayerDodgeState(this, StateMachine);

            Assert.IsNotNull(baseStats, "Base Stats SO is not assigned.");
            m_stats = baseStats.data.Clone();

            StateMachine.Initialize(IdleState);
            SetPrimaryWeapon(defaultPrimaryWeapon);
            SetSecondaryWeapon(defaultSecondaryWeapon);
        }

        private void Update()
        {
            StateMachine.Update();
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        private void SetPrimaryWeapon(BaseWeaponSO newWeapon)
        {
            Assert.IsTrue(newWeapon.Type == WeaponType.Primary, "Provided weapon is not of type Primary.");
            if (PrimaryWeaponGO != null)
            {
                Destroy(PrimaryWeaponGO);
            }

            PrimaryWeaponSO = newWeapon;
            if (newWeapon.WeaponPrefab != null)
            {
                PrimaryWeaponGO = Instantiate(newWeapon.WeaponPrefab, GetHandSocketTransform(newWeapon));
                PrimaryWeaponGO.SetActive(false);
            }
            Assert.IsTrue(
                StateMachine.CurrentState != PrimaryAttackState,
                "Trying to set primary weapon in the middle of a primary attack."
            );
            PrimaryAttackState = PrimaryWeaponSO switch
            {
                RangedWeaponSO rangedWeapon => new PlayerRangedAttackState(this, StateMachine, rangedWeapon, PrimaryWeaponGO),
                MeleeWeaponSO meleeWeapon => new PlayerMeleeAttackState(this, StateMachine, meleeWeapon, PrimaryWeaponGO),
                _ => throw new NotImplementedException($"Weapon type {newWeapon.Type} not implemented.")
            };
        }

        private void SetSecondaryWeapon(BaseWeaponSO newWeapon)
        {
            Assert.IsTrue(newWeapon.Type == WeaponType.Secondary, "Provided weapon is not of type Secondary.");
            if (SecondaryWeaponGO != null)
            {
                Destroy(SecondaryWeaponGO);
            }

            SecondaryWeaponSO = newWeapon;
            if (newWeapon.WeaponPrefab != null)
            {
                SecondaryWeaponGO = Instantiate(newWeapon.WeaponPrefab, GetHandSocketTransform(newWeapon));
                SecondaryWeaponGO.SetActive(false);
            }
            Assert.IsTrue(
                StateMachine.CurrentState != SecondaryAttackState,
                "Trying to set secondary weapon in the middle of a secondary attack."
            );
            SecondaryAttackState = SecondaryWeaponSO switch
            {
                RangedWeaponSO rangedWeapon => new PlayerRangedAttackState(this, StateMachine, rangedWeapon, SecondaryWeaponGO),
                MeleeWeaponSO meleeWeapon => new PlayerMeleeAttackState(this, StateMachine, meleeWeapon, SecondaryWeaponGO),
                _ => throw new NotImplementedException($"Weapon type {newWeapon.Type} not implemented.")
            };
        }

        private Transform GetHandSocketTransform(BaseWeaponSO weapon)
        {
            return weapon.SocketHand == HandSocket.Left ? leftHandSocket : rightHandSocket;
        }

        public void HideWeapons()
        {
            PrimaryWeaponGO?.SetActive(false);
            SecondaryWeaponGO?.SetActive(false);
        }

        public void EnableActorCollision(bool enable)
        {
            RB.excludeLayers = enable ? 0 : m_actorsMask;
        }

        public float Health
        {
            get => m_stats.health;
            set => m_stats.health = Mathf.Clamp(value, 0, m_stats.maxHealth);
        }

        public float MovementVelocity => m_stats.movementVelocity;

        public float DodgeVelocity => m_stats.dodgeVelocity;

        public float DodgeCooldown => m_stats.dodgeCooldown;
    }
}

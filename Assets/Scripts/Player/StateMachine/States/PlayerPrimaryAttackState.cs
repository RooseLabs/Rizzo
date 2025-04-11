using RooseLabs.ScriptableObjects;
using UnityEngine;

namespace RooseLabs.Player.StateMachine.States
{
    public class PlayerPrimaryAttackState : PlayerAttackBaseState
    {
        public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        protected override bool PressedAttack => Player.InputHandler.PressedPrimaryAttack;
        protected override BaseWeaponSO WeaponData => Player.PrimaryWeaponSO;
        protected override GameObject WeaponGO => Player.PrimaryWeaponGO;
    }
}

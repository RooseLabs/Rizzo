using RooseLabs.ScriptableObjects;
using UnityEngine;

namespace RooseLabs.Player.StateMachine.States
{
    public class PlayerSecondaryAttackState : PlayerAttackBaseState
    {
        public PlayerSecondaryAttackState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        protected override bool PressedAttack => Player.InputHandler.PressedSecondaryAttack;
        protected override BaseWeaponSO WeaponData => Player.SecondaryWeaponSO;
        protected override GameObject WeaponGO => Player.SecondaryWeaponGO;
    }
}

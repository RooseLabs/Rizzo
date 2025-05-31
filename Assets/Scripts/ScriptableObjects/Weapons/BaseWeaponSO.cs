using RooseLabs.Enums;
using RooseLabs.Models;
using UnityEngine;

namespace RooseLabs.ScriptableObjects.Weapons
{
    public abstract class BaseWeaponSO : ScriptableObject
    {
        [SerializeField] protected WeaponType weaponType;
        [SerializeField] protected string weaponName;
        [SerializeField] protected GameObject weaponPrefab;
        [SerializeField] protected Sprite weaponIcon;
        [SerializeField] protected HandSocket socketHand;
        [SerializeField] protected RuntimeAnimatorController animatorController;
        [SerializeField] protected WeaponAttackData[] attacks;
        [SerializeField] protected bool dodgeResetsCombo;

        public WeaponType Type => weaponType;
        public string Name => weaponName;
        public GameObject WeaponPrefab => weaponPrefab;
        public Sprite Icon => weaponIcon;
        public HandSocket SocketHand => socketHand;
        public WeaponAttackData[] Attacks => attacks;
        public bool DodgeResetsCombo => dodgeResetsCombo;
    }
}

using RooseLabs.Enums;
using RooseLabs.Models;
using UnityEngine;
using UnityEditor.Animations;

namespace RooseLabs.ScriptableObjects
{
    public abstract class BaseWeaponSO : ScriptableObject
    {
        [SerializeField] protected string weaponName;
        [SerializeField] protected GameObject weaponPrefab;
        [SerializeField] protected Sprite weaponIcon;
        [SerializeField] protected HandSocket socketHand;
        [SerializeField] protected AnimatorController animatorController;
        [SerializeField] protected WeaponAttackData[] attacks;
        [SerializeField] protected bool dodgeResetsCombo;

        public string WeaponName => weaponName;
        public GameObject WeaponPrefab => weaponPrefab;
        public Sprite WeaponIcon => weaponIcon;
        public HandSocket SocketHand => socketHand;
        public WeaponAttackData[] Attacks => attacks;
        public bool DodgeResetsCombo => dodgeResetsCombo;
    }
}

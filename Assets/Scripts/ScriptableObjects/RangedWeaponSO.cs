using RooseLabs.Models;
using UnityEngine;

namespace RooseLabs.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RangedWeapon", menuName = "Data/Ranged Weapon")]
    public class RangedWeaponSO : BaseWeaponSO
    {
        [Header("Ranged Weapon Data")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private AnimationStateData aimAnimationState;
        [SerializeField] private bool canHoldToAim;

        [SerializeField] private AnimationStateData reloadAnimationState;
        [Tooltip("When true, the weapon will reload in the background without needing to play the reload animation.")]
        [SerializeField] private bool autoReload;
        [Tooltip("The time in seconds that it takes to fully reload the weapon in the background when auto reload is enabled.")]
        [SerializeField] private float reloadDuration = 1f;
        [Tooltip("When true, projectiles/bullets are reloaded one at a time over the reload duration.")]
        [SerializeField] private bool progressiveReload = true;

        [Tooltip("The maximum amount of projectiles/bullets before needing to reload.")]
        [SerializeField] private int maxAmmunition = 10;
        [Tooltip("How far a projectile/bullet can travel before it is destroyed.")]
        [SerializeField] private float attackRange = 10f;

        [Tooltip("Whether the weapon is automatic (shoots continuously while the respective attack button is held down). " +
                 "The delay between shots is determined by the minimum combo time of each attack in the Attacks list.")]
        [SerializeField] private bool isAutomatic;

        public GameObject ProjectilePrefab => projectilePrefab;
        public AnimationStateData AimAnimationState => aimAnimationState;
        public bool CanHoldToAim => canHoldToAim;
        public AnimationStateData ReloadAnimationState => reloadAnimationState;
        public bool AutoReload => autoReload;
        public float ReloadDuration => reloadDuration;
        public bool ProgressiveReload => progressiveReload;
        public int MaxAmmunition => maxAmmunition;
        public float AttackRange => attackRange;
        public bool IsAutomatic => isAutomatic;
    }
}

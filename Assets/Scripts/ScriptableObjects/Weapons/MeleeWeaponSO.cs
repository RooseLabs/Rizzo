using RooseLabs.Gameplay.Combat;
using UnityEngine;

namespace RooseLabs.ScriptableObjects.Weapons
{
    [CreateAssetMenu(fileName = "MeleeWeapon", menuName = "Weapon/Melee Weapon")]
    public class MeleeWeaponSO : BaseWeaponSO
    {
        [Header("Melee Weapon Data")]
        [SerializeField] private GameObject hitboxPrefab;

        public GameObject HitboxPrefab => hitboxPrefab;

        private void OnValidate()
        {
            if (hitboxPrefab != null && hitboxPrefab.GetComponent<Hitbox>() == null)
            {
                Debug.LogWarning("Hitbox prefab must contain a Hitbox component.", this);
            }
        }
    }
}

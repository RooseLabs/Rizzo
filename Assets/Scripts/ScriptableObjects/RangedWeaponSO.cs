using RooseLabs.Models;
using UnityEngine;

namespace RooseLabs.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RangedWeapon", menuName = "Data/Ranged Weapon")]
    public class RangedWeaponSO : BaseWeaponSO
    {
        [Header("Ranged Weapon Data")]
        [SerializeField] private AnimationStateData aimAnimationState;
    }
}

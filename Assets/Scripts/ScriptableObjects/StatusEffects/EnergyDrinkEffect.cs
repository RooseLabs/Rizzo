using RooseLabs.Models;
using UnityEngine;

namespace RooseLabs.ScriptableObjects.StatusEffects
{
    public class EnergyDrinkEffect : StatusEffectSO
    {
        [SerializeField] protected StatEffect moveSpeedEffect;

        public override string Description => $"{effectName}: {string.Format(effectDescription, moveSpeedEffect.baseValue)}";

        public override void Apply(PlayerStats stats)
        {
            base.Apply(stats);
            stats.movementVelocity += GetFinalStatEffect(stats.movementVelocity, moveSpeedEffect);
        }
    }
}

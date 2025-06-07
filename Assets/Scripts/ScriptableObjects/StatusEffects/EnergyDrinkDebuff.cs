using RooseLabs.Enums;
using RooseLabs.Models;
using UnityEngine;

namespace RooseLabs.ScriptableObjects.StatusEffects
{
    public class EnergyDrinkDebuff : EnergyDrinkEffect
    {
        [SerializeField] private StatEffect healthLossEffect;

        private int m_obstacleLayer;

        private void OnEnable()
        {
            m_obstacleLayer = LayerMask.NameToLayer("Obstacle");
        }

        public override StatusEffectType Type => StatusEffectType.Debuff;

        public override string Description => $"{effectName}: {string.Format(effectDescription, moveSpeedEffect.baseValue, healthLossEffect.baseValue)}";

        public override void OnCollision(PlayerStats stats, Collision2D collision)
        {
            if (collision.gameObject.layer != m_obstacleLayer) return;
            stats.health -= GetFinalStatEffect(stats.health, healthLossEffect);
        }
    }
}

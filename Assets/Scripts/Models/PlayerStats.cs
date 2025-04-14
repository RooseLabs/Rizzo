using System;
using UnityEngine;

namespace RooseLabs.Models
{
    [Serializable]
    public class PlayerStats
    {
        public float health = 80f;
        public float maxHealth = 80f;

        [Header("Movement")]
        public float movementVelocity = 2.5f;

        [Header("Dodge")]
        public float dodgeVelocity = 2.5f;
        public float dodgeCooldown = 0.5f;

        public PlayerStats() { }

        private PlayerStats(PlayerStats stats)
        {
            health = stats.health;
            maxHealth = stats.maxHealth;
            movementVelocity = stats.movementVelocity;
            dodgeVelocity = stats.dodgeVelocity;
            dodgeCooldown = stats.dodgeCooldown;
        }

        public PlayerStats Clone()
        {
            return new PlayerStats(this);
        }
    }
}

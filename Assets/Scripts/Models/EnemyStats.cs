using System;

namespace RooseLabs.Models
{
    [Serializable]
    public class EnemyStats
    {
        public float health = 100f;
        public float maxHealth = 100f;
        public float movementVelocity = 2.5f;

        public EnemyStats() { }

        private EnemyStats(EnemyStats stats)
        {
            health = stats.health;
            maxHealth = stats.maxHealth;
            movementVelocity = stats.movementVelocity;
        }

        public EnemyStats Clone()
        {
            return new EnemyStats(this);
        }
    }
}

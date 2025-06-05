using RooseLabs.Gameplay.Combat;
using UnityEngine;

namespace RooseLabs.Enemies
{
    public abstract class BaseEnemy : MonoBehaviour, IDamageable
    {
        public abstract void ApplyDamage(float damage);
        public abstract float Health { get; protected set; }
    }
}

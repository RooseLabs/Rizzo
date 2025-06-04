using RooseLabs.ScriptableObjects.Enemies;
using UnityEngine;

namespace RooseLabs.Gameplay.SpawnSystem
{
    public class EnemySpawnLocation : MonoBehaviour
    {
        [Tooltip("The wave number this spawn location belongs to. Used for spawning enemies in waves.")]
        [Range(1, 10)][SerializeField] private int wave = 0;

        [Tooltip("List of enemy types that can spawn at this location.")]
        [SerializeField] private BaseEnemySO[] enemyPool;

        public Vector3 Position => transform.position;
        public int Wave => wave;
        public BaseEnemySO[] EnemyPool => enemyPool;
    }
}

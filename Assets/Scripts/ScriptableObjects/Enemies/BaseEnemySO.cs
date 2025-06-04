using RooseLabs.Models;
using UnityEngine;

namespace RooseLabs.ScriptableObjects.Enemies
{
    public abstract class BaseEnemySO : DescriptionBaseSO
    {
        [SerializeField] protected GameObject enemyPrefab;
        [SerializeField] protected EnemyStats stats;

        public GameObject EnemyPrefab => enemyPrefab;
        public EnemyStats Stats => stats;

        public abstract void Spawn(Vector3 position);
    }
}

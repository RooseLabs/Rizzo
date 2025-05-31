using RooseLabs.Models;
using UnityEngine;

namespace RooseLabs.ScriptableObjects.Enemies
{
    public class BaseEnemySO : ScriptableObject
    {
        [SerializeField][TextArea] protected string description;
        [SerializeField] protected GameObject enemyPrefab;
        [SerializeField] protected EnemyStats stats;

        public GameObject EnemyPrefab => enemyPrefab;
        public EnemyStats Stats => stats;
    }
}

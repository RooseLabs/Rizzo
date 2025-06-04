using RooseLabs.Models;
using RooseLabs.ScriptableObjects.Enemies;
using UnityEngine;

namespace RooseLabs.Enemies
{
    [RequireComponent(typeof(Actor3D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class Enemy<T> : MonoBehaviour where T : BaseEnemySO
    {
        private readonly Collider2D[] m_separationBuffer = new Collider2D[16];
        private ContactFilter2D m_enemyContactFilter;

        public LayerMask PlayerLayerMask { get; private set; }
        public LayerMask EnemyLayerMask { get; private set; }
        public LayerMask ObstacleLayerMask { get; private set; }
        public int PlayerLayerIndex { get; private set; }
        public int EnemyLayerIndex { get; private set; }
        public int ObstacleLayerIndex { get; private set; }

        #region Components
        public Actor3D Actor3D { get; private set; }
        public Animator Animator { get; private set; }
        public Rigidbody2D RB { get; private set; }
        public Collider2D Collider { get; private set; }
        #endregion

        public EnemyStats Stats { get; private set; }

        protected virtual void Awake()
        {
            PlayerLayerMask = LayerMask.GetMask("Player");
            EnemyLayerMask = LayerMask.GetMask("Enemy");
            ObstacleLayerMask = LayerMask.GetMask("Obstacle");
            PlayerLayerIndex = LayerMask.NameToLayer("Player");
            EnemyLayerIndex = LayerMask.NameToLayer("Enemy");
            ObstacleLayerIndex = LayerMask.NameToLayer("Obstacle");

            m_enemyContactFilter = new ContactFilter2D { layerMask = EnemyLayerMask, useTriggers = false, useLayerMask = true};

            Actor3D = GetComponent<Actor3D>();
            Animator = GetComponentInChildren<Animator>();
            RB = GetComponent<Rigidbody2D>();
            Collider = GetComponent<Collider2D>();
        }

        protected virtual void Initialize(T enemyData)
        {
            Stats = enemyData.Stats.Clone();
        }

        public static Enemy<T> Create(T enemyData, Vector3 position)
        {
            GameObject enemyGO = Instantiate(enemyData.EnemyPrefab, position, enemyData.EnemyPrefab.transform.rotation);
            Enemy<T> enemy = enemyGO.GetComponent<Enemy<T>>();
            enemy.Initialize(enemyData);
            return enemy;
        }

        /// <summary>
        ///   Calculates a separation vector to help prevent this enemy from overlapping with other enemies.
        ///   Uses the Collider2D.Overlap method to detect nearby enemies (on the Enemy layer),
        ///   and computes a repelling vector based on their positions.
        /// </summary>
        /// <param name="strength">Controls how strong the separation effect (repelling force) is.</param>
        /// <returns>A normalized vector pointing away from nearby enemies, scaled by the given strength.</returns>
        public Vector2 GetSeparationVector(float strength = 0.25f)
        {
            int hitCount = Collider.Overlap(m_enemyContactFilter, m_separationBuffer);
            if (hitCount == 0) return Vector2.zero;

            Vector2 separation = Vector2.zero;
            int count = 0;
            for (int i = 0; i < hitCount; i++)
            {
                var hit = m_separationBuffer[i];
                if (hit == Collider) continue;
                separation += (Vector2)hit.transform.position;
                count++;
            }

            if (count > 0)
            {
                separation /= count;
                separation = ((Vector2)transform.position - separation).normalized;
            }
            return separation * strength;
        }
    }
}

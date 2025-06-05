using System.Collections;
using RooseLabs.Gameplay.Combat;
using RooseLabs.Models;
using RooseLabs.ScriptableObjects.Enemies;
using UnityEngine;

namespace RooseLabs.Enemies
{
    [RequireComponent(typeof(Actor3D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class Enemy<T> : MonoBehaviour, IDamageable where T : BaseEnemySO
    {
        private bool s_staticDataInitialized = false;

        private readonly Collider2D[] m_separationBuffer = new Collider2D[16];
        private static ContactFilter2D s_enemyContactFilter;

        private const float DamageFeedbackDuration = 0.1f;
        private static Color s_damageFeedbackColor;

        public static LayerMask PlayerLayerMask { get; private set; }
        public static LayerMask EnemyLayerMask { get; private set; }
        public static LayerMask ObstacleLayerMask { get; private set; }
        public static int PlayerLayerIndex { get; private set; }
        public static int EnemyLayerIndex { get; private set; }
        public static int ObstacleLayerIndex { get; private set; }

        #region Components
        public Actor3D Actor3D { get; private set; }
        public Animator Animator { get; private set; }
        public Rigidbody2D RB { get; private set; }
        public Collider2D Collider { get; private set; }
        #endregion

        public T EnemyData { get; private set; }
        public EnemyStats Stats { get; private set; }

        protected virtual void Awake()
        {
            if (!s_staticDataInitialized)
            {
                PlayerLayerMask = LayerMask.GetMask("Player");
                EnemyLayerMask = LayerMask.GetMask("Enemy");
                ObstacleLayerMask = LayerMask.GetMask("Obstacle");
                PlayerLayerIndex = LayerMask.NameToLayer("Player");
                EnemyLayerIndex = LayerMask.NameToLayer("Enemy");
                ObstacleLayerIndex = LayerMask.NameToLayer("Obstacle");
                s_enemyContactFilter = new ContactFilter2D { layerMask = EnemyLayerMask, useTriggers = false, useLayerMask = true};
                s_damageFeedbackColor = new Color(1f, 0.4f, 0.4f, 1f);
                s_staticDataInitialized = true;
            }

            Actor3D = GetComponent<Actor3D>();
            Animator = GetComponentInChildren<Animator>();
            RB = GetComponent<Rigidbody2D>();
            Collider = GetComponent<Collider2D>();
        }

        protected virtual void Initialize(T enemyData)
        {
            EnemyData = enemyData;
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
            int hitCount = Collider.Overlap(s_enemyContactFilter, m_separationBuffer);
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

        public void ApplyDamage(float damage)
        {
            Health -= damage;
            DamageFeedback();
        }

        private void DamageFeedback()
        {
            if (Actor3D.Materials != null && Actor3D.MaterialOriginalColors != null)
            {
                StartCoroutine(DamageFeedbackRoutine());
            }
        }

        private IEnumerator DamageFeedbackRoutine()
        {
            // Set all material colors to damage feedback color
            for (int i = 0; i < Actor3D.Materials.Length; i++)
            {
                Actor3D.Materials[i].color = s_damageFeedbackColor;
            }

            yield return new WaitForSeconds(DamageFeedbackDuration);

            // Restore original colors
            for (int i = 0; i < Actor3D.Materials.Length; i++)
            {
                Actor3D.Materials[i].color = Actor3D.MaterialOriginalColors[i];
            }
        }

        protected virtual void OnDeath()
        {
            Actor3D.FadeOut();
        }

        protected float Health
        {
            get => Stats.health;
            set
            {
                Stats.health = Mathf.Clamp(value, 0f, Stats.maxHealth);
                if (Stats.health <= 0f) OnDeath();
            }
        }
    }
}

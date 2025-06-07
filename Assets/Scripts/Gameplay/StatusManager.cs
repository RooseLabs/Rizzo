using System;
using System.Collections.Generic;
using System.Linq;
using RooseLabs.Events.Channels;
using RooseLabs.Models;
using RooseLabs.ScriptableObjects;
using RooseLabs.ScriptableObjects.StatusEffects;
using UnityEngine;

namespace RooseLabs.Gameplay
{
    public class StatusManager : MonoBehaviour
    {
        [SerializeField] private PlayerStatsSO playerBaseStats;

        [Header("Listening to")]
        [SerializeField] private TransformEventChannelSO playerInstantiatedChannel;

        private Player.Player m_player;
        private PlayerStats CurrentStats { get; set; } // StatusManager owns the player's stats
        private List<Tuple<ItemSO, StatusEffectSO>> EquippedItems { get; set; } = new();
        private List<StatusEffectSO> ActiveStatusEffects { get; set; } = new();

        private void OnEnable()
        {
            Player.Player.OnUpdate += OnPlayerUpdate;
            Player.Player.OnFixedUpdate += OnPlayerFixedUpdate;
            Player.Player.OnLateUpdate += OnPlayerLateUpdate;
            Player.Player.OnCollision += OnPlayerCollision;
            Player.Player.OnTriggerCollision += OnPlayerTriggerCollision;
            playerInstantiatedChannel.OnEventRaised += OnPlayerInstantiated;
        }

        private void OnDisable()
        {
            playerInstantiatedChannel.OnEventRaised -= OnPlayerInstantiated;
            Player.Player.OnUpdate -= OnPlayerUpdate;
            Player.Player.OnFixedUpdate -= OnPlayerFixedUpdate;
            Player.Player.OnLateUpdate -= OnPlayerLateUpdate;
            Player.Player.OnCollision -= OnPlayerCollision;
            Player.Player.OnTriggerCollision -= OnPlayerTriggerCollision;
        }

        public void Initialize()
        {
            CurrentStats = playerBaseStats.data.Clone();
        }

        private void OnStatusEffectAdded(StatusEffectSO statusEffect)
        {
            if (!statusEffect.IsStackable && ActiveStatusEffects.Any(se => se == statusEffect)) return;

            ActiveStatusEffects.Add(statusEffect);
            statusEffect.Apply(CurrentStats);
        }

        private void OnPlayerUpdate()
        {
            foreach (var statusEffect in ActiveStatusEffects)
            {
                statusEffect.OnUpdate(CurrentStats);
            }
        }

        private void OnPlayerFixedUpdate()
        {
            foreach (var statusEffect in ActiveStatusEffects)
            {
                statusEffect.OnFixedUpdate(CurrentStats);
            }
        }

        private void OnPlayerLateUpdate()
        {
            var activeEffects = ActiveStatusEffects.ToList();
            foreach (var statusEffect in activeEffects)
            {
                if (statusEffect.IsPermanent) continue;
                statusEffect.CurrentDuration -= Time.deltaTime;
                if (statusEffect.CurrentDuration <= 0.0f)
                {
                    ActiveStatusEffects.Remove(statusEffect);
                }
            }
        }

        private void OnPlayerCollision(Collision2D collision)
        {
            foreach (var statusEffect in ActiveStatusEffects)
            {
                statusEffect.OnCollision(CurrentStats, collision);
            }
        }

        private void OnPlayerTriggerCollision(Collider2D tCollider)
        {
            foreach (var statusEffect in ActiveStatusEffects)
            {
                statusEffect.OnTriggerCollision(CurrentStats, tCollider);
            }
        }

        private void OnPlayerInstantiated(Transform playerTransform)
        {
            m_player = playerTransform.GetComponent<Player.Player>();
            m_player.Initialize(CurrentStats);
        }

        public float Health
        {
            get => CurrentStats.health;
            set => CurrentStats.health = Mathf.Clamp(value, 0, CurrentStats.maxHealth);
        }
    }
}

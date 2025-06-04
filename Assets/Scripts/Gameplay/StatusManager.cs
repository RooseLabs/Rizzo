using RooseLabs.Events.Channels;
using RooseLabs.Models;
using RooseLabs.ScriptableObjects;
using UnityEngine;

namespace RooseLabs.Gameplay
{
    public class StatusManager : MonoBehaviour
    {
        [SerializeField] private PlayerStatsSO playerBaseStats;

        [Header("Listening to")]
        [SerializeField] private TransformEventChannelSO playerInstantiatedChannel;

        private Player.Player Player { get; set; }
        private PlayerStats CurrentStats { get; set; } // StatusManager owns the player's stats

        private void OnEnable()
        {
            playerInstantiatedChannel.OnEventRaised += OnPlayerInstantiated;
        }

        private void OnDisable()
        {
            playerInstantiatedChannel.OnEventRaised -= OnPlayerInstantiated;
        }

        public void Initialize()
        {
            CurrentStats = playerBaseStats.data.Clone();
        }

        private void OnPlayerInstantiated(Transform playerTransform)
        {
            Player = playerTransform.GetComponent<Player.Player>();
            Player.Initialize(CurrentStats);
        }

        public float Health
        {
            get => CurrentStats.health;
            set => CurrentStats.health = Mathf.Clamp(value, 0, CurrentStats.maxHealth);
        }
    }
}

using RooseLabs.Events.Channels;
using RooseLabs.Generics;
using UnityEngine;

namespace RooseLabs.Gameplay
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [Header("Listening to")]
        [SerializeField] private TransformEventChannelSO playerInstantiatedChannel;
        [SerializeField] private VoidEventChannelSO onSceneReady;

        [Header("Managers")]
        [SerializeField] private StatusManager statusManager;

        public Player.Player Player { get; private set; }
        public int CurrentChamberNumber { get; private set; } = 1;

        private void OnEnable()
        {
            playerInstantiatedChannel.OnEventRaised += OnPlayerInstantiated;
            onSceneReady.OnEventRaised += OnNewChamber;
            StartRun();
        }

        private void OnDisable()
        {
            playerInstantiatedChannel.OnEventRaised -= OnPlayerInstantiated;
            onSceneReady.OnEventRaised -= OnNewChamber;
        }

        private void StartRun()
        {
            statusManager.Initialize();
        }

        private void OnNewChamber()
        {
            CurrentChamberNumber++;
        }

        private void OnPlayerInstantiated(Transform playerTransform)
        {
            Player = playerTransform.GetComponent<Player.Player>();
        }
    }
}

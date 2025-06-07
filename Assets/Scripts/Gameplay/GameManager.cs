using RooseLabs.Events.Channels;
using UnityEngine;

namespace RooseLabs.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private StatusManager statusManager;

        [Header("Listening to")]
        [SerializeField] private TransformEventChannelSO playerInstantiatedChannel;
        [SerializeField] private VoidEventChannelSO onSceneReady;

        public Player.Player Player { get; private set; }
        public int CurrentChamberNumber { get; private set; } = 1;

        public static GameManager Instance { get; private set; }

        private void OnEnable()
        {
            Instance = this;
            playerInstantiatedChannel.OnEventRaised += OnPlayerInstantiated;
            onSceneReady.OnEventRaised += OnNewChamber;
            StartRun();
        }

        private void OnDisable()
        {
            playerInstantiatedChannel.OnEventRaised -= OnPlayerInstantiated;
            onSceneReady.OnEventRaised -= OnNewChamber;
            Instance = null;
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

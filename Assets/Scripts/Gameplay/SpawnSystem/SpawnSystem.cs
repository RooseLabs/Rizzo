using RooseLabs.Events.Channels;
using UnityEngine;

namespace RooseLabs.Gameplay.SpawnSystem
{
    public class SpawnSystem : MonoBehaviour
    {
        [SerializeField] private Player.Player playerPrefab;

        [Header("Broadcasting on")]
        [SerializeField] private TransformEventChannelSO playerInstantiatedChannel;

        [Header("Listening to")]
        [SerializeField] private VoidEventChannelSO onSceneReady;

        private Transform m_spawnPoint;
        private EnemySpawnLocation[] m_enemySpawnLocations;

        private void Awake()
        {
            m_spawnPoint = transform.GetChild(0);
            m_enemySpawnLocations = transform.GetComponentsInChildren<EnemySpawnLocation>();
        }

        private void OnEnable()
        {
            onSceneReady.OnEventRaised += OnSceneReady;
        }

        private void OnDisable()
        {
            onSceneReady.OnEventRaised -= OnSceneReady;
        }

        private void OnSceneReady()
        {
            SpawnPlayer();
            SpawnEnemies(1);
        }

        private void SpawnPlayer()
        {
            var playerInstance = Instantiate(playerPrefab, m_spawnPoint.position, playerPrefab.transform.rotation);
            playerInstantiatedChannel.RaiseEvent(playerInstance.transform);
        }

        private void SpawnEnemies(int wave)
        {
            foreach (var spawnLocation in m_enemySpawnLocations)
            {
                if (spawnLocation.Wave != wave) continue;
                // Get random enemy from the pool
                var enemyPool = spawnLocation.EnemyPool;
                if (enemyPool.Length == 0) continue;
                var randomIndex = Random.Range(0, enemyPool.Length);
                var enemyData = enemyPool[randomIndex];
                // Spawn the enemy
                enemyData.Spawn(spawnLocation.Position);
            }
        }
    }
}

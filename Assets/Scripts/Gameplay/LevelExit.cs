using Eflatun.SceneReference;
using RooseLabs.Enemies;
using RooseLabs.Events.Channels;
using UnityEngine;

namespace RooseLabs.Gameplay
{
    public class LevelExit : MonoBehaviour
    {
        [SerializeField] private SceneReference nextLevelScene;
        [SerializeField] private LoadEventChannelSO loadLevelChannel;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (!AreAllEnemiesDead()) return;
            loadLevelChannel.RaiseEvent(nextLevelScene, false, true);
        }

        private static bool AreAllEnemiesDead()
        {
            // Find all enemies in the scene
            var enemies = FindObjectsByType<BaseEnemy>(FindObjectsSortMode.None);

            if (enemies.Length == 0) return true;
            foreach (var enemy in enemies)
            {
                if (enemy.Health > 0) return false;
            }
            return true;
        }
    }
}

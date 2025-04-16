using Eflatun.SceneReference;
using RooseLabs.Events.Channels;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RooseLabs.SceneManagement
{
    public class InitializationLoader : MonoBehaviour
    {
        [SerializeField] private SceneReference managersScene;
        [SerializeField] private SceneReference mainMenuScene;

        [Header("Broadcasting on")]
        [SerializeField] private LoadEventChannelSO loadMenuChannel;

        private void Start()
        {
            var operation = SceneManager.LoadSceneAsync(managersScene.Path, LoadSceneMode.Additive);
            if (operation != null)
            {
                operation.completed += LoadMainMenu;
            }
            else
            {
                Debug.LogError("[InitializationLoader] Failed to load Persistent Managers scene.");
            }
        }

        private void LoadMainMenu(AsyncOperation op)
        {
            loadMenuChannel.RaiseEvent(mainMenuScene);
            SceneManager.UnloadSceneAsync(0);
        }
    }
}

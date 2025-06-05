using Eflatun.SceneReference;
using RooseLabs.Events.Channels;
using RooseLabs.Input;
using UnityEngine;

namespace RooseLabs.UI.Gameplay
{
    public class UIGameOverManager : MonoBehaviour
    {
        [SerializeField] private UIGameOverScreen gameOverScreen;
        [SerializeField] private SceneReference mainMenuScene;

        [Header("Listening to")]
        [SerializeField] private VoidEventChannelSO onPlayerDeath;

        [Header("Broadcasting on")]
        [SerializeField] private LoadEventChannelSO loadMenuChannel;

        private void OnEnable()
        {
            onPlayerDeath.OnEventRaised += OpenGameOverScreen;
        }

        private void OnDisable()
        {
            onPlayerDeath.OnEventRaised -= OpenGameOverScreen;
        }

        private void OpenGameOverScreen()
        {
            InputManager.Instance.DisableAllInput();
            gameOverScreen.OnGameOverAnimationComplete += BackToMainMenu;
            gameOverScreen.gameObject.SetActive(true);
        }

        private void BackToMainMenu()
        {
            gameOverScreen.OnGameOverAnimationComplete -= BackToMainMenu;
            loadMenuChannel.RaiseEvent(mainMenuScene, true, true);
        }
    }
}

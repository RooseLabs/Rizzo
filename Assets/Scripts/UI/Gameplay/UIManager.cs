using Eflatun.SceneReference;
using RooseLabs.Events.Channels;
using RooseLabs.Input;
using UnityEngine;

namespace RooseLabs.UI.Gameplay
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private SceneReference mainMenuScene;

        [Header("UI Elements")]
        [SerializeField] private GameObject hudPanel;
        [SerializeField] private UIPause pauseScreen;

        [Header("Listening to")]
        [SerializeField] private VoidEventChannelSO onSceneReady;

        [Header("Broadcasting on")]
        [SerializeField] private LoadEventChannelSO loadMenuChannel;

        private InputManager inputManager;

        private void Awake()
        {
            inputManager = InputManager.Instance;
        }

        private void OnEnable()
        {
            onSceneReady.OnEventRaised += ResetUI;
            inputManager.MenuPauseEvent += OpenPauseScreen;
        }

        private void OnDisable()
        {
            onSceneReady.OnEventRaised -= ResetUI;
            inputManager.MenuPauseEvent -= OpenPauseScreen;
        }

        private void ResetUI()
        {
            hudPanel.SetActive(true);
            pauseScreen.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }

        private void OpenPauseScreen()
        {
            inputManager.MenuPauseEvent -= OpenPauseScreen; // Prevent opening the pause menu again if it's already open
            inputManager.MenuUnpauseEvent += ClosePauseScreen; // Allow closing the pause menu via ESC / Start button

            Time.timeScale = 0; // Pause time

            pauseScreen.BackToMainRequested += BackToMainMenu;
            pauseScreen.ResumeRequested += ClosePauseScreen;

            pauseScreen.gameObject.SetActive(true);

            inputManager.EnableMenuInput();
        }

        private void ClosePauseScreen()
        {
            pauseScreen.BackToMainRequested -= BackToMainMenu;
            pauseScreen.ResumeRequested -= ClosePauseScreen;

            pauseScreen.gameObject.SetActive(false);

            inputManager.MenuPauseEvent += OpenPauseScreen; // Allow opening the pause menu again
            inputManager.MenuUnpauseEvent -= ClosePauseScreen;

            inputManager.EnableGameplayInput();
            Time.timeScale = 1; // Unpause time
        }

        private void BackToMainMenu()
        {
            ClosePauseScreen();
            hudPanel.SetActive(false);
            loadMenuChannel.RaiseEvent(mainMenuScene, false, true);
        }
    }
}

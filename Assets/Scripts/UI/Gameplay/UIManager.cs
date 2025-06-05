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
        [SerializeField] private HUDManager hudPanel;
        [SerializeField] private UIPause pauseScreen;

        [Header("Listening to")]
        [SerializeField] private VoidEventChannelSO onSceneReady;

        [Header("Broadcasting on")]
        [SerializeField] private LoadEventChannelSO loadMenuChannel;

        private InputManager m_inputManager;
        private bool m_isPauseScreenOpen;

        private void Awake()
        {
            m_inputManager = InputManager.Instance;
        }

        private void OnEnable()
        {
            onSceneReady.OnEventRaised += ResetUI;
            m_inputManager.MenuPauseEvent += OpenPauseScreen;
            m_inputManager.MenuUnpauseEvent += ClosePauseScreen;
        }

        private void OnDisable()
        {
            onSceneReady.OnEventRaised -= ResetUI;
            m_inputManager.MenuPauseEvent -= OpenPauseScreen;
            m_inputManager.MenuUnpauseEvent -= ClosePauseScreen;
        }

#if !UNITY_EDITOR
        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus && !m_isPauseScreenOpen)
                OpenPauseScreen();
        }
#endif

        private void ResetUI()
        {
            hudPanel.gameObject.SetActive(true);
            pauseScreen.gameObject.SetActive(false);
            m_isPauseScreenOpen = false;
            Time.timeScale = 1f;
        }

        private void OpenPauseScreen()
        {
            if (m_isPauseScreenOpen) return;
            m_isPauseScreenOpen = true;

            Time.timeScale = 0; // Pause time

            pauseScreen.BackToMainRequested += BackToMainMenu;
            pauseScreen.ResumeRequested += ClosePauseScreen;

            pauseScreen.gameObject.SetActive(true);

            m_inputManager.EnableMenuInput();
        }

        private void ClosePauseScreen()
        {
            if (!m_isPauseScreenOpen) return;
            m_isPauseScreenOpen = false;

            pauseScreen.BackToMainRequested -= BackToMainMenu;
            pauseScreen.ResumeRequested -= ClosePauseScreen;

            pauseScreen.gameObject.SetActive(false);

            m_inputManager.EnableGameplayInput();
            Time.timeScale = 1; // Unpause time
        }

        private void BackToMainMenu()
        {
            ClosePauseScreen();
            hudPanel.gameObject.SetActive(false);
            loadMenuChannel.RaiseEvent(mainMenuScene, false, true);
        }
    }
}

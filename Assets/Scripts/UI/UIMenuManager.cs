using Eflatun.SceneReference;
using RooseLabs.Events.Channels;
using RooseLabs.Input;
using UnityEngine;

namespace RooseLabs.UI
{
    public class UIMenuManager : MonoBehaviour
    {
        [SerializeField] private UIMainMenu mainMenuPanel;
        [SerializeField] private GameObject archivePanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject creditsPanel;

        [SerializeField] private SceneReference levelToLoad;

        [Header("Broadcasting on")]
        [SerializeField] private LoadEventChannelSO loadLevelChannel;

        private void Start()
        {
            InputManager.Instance.EnableMenuInput();
            mainMenuPanel.PlayButtonAction += PlayButtonClicked;
            mainMenuPanel.ArchiveButtonAction += OpenArchiveScreen;
            mainMenuPanel.SettingsButtonAction += OpenSettingsScreen;
            mainMenuPanel.CreditsButtonAction += OpenCreditsScreen;
            mainMenuPanel.ExitButtonAction += ExitGame;
        }

        private void PlayButtonClicked()
        {
            loadLevelChannel.RaiseEvent(levelToLoad, true, true);
        }

        private void OpenArchiveScreen()
        {
            // mainMenuPanel.SetActive(false);
            // archivePanel.SetActive(true);
        }

        private void OpenSettingsScreen()
        {
            // mainMenuPanel.SetActive(false);
            // settingsPanel.SetActive(true);
        }

        private void OpenCreditsScreen()
        {
            // mainMenuPanel.SetActive(false);
            // creditsPanel.SetActive(true);
        }

        private void ExitGame()
        {
            Application.Quit();
        }
    }
}

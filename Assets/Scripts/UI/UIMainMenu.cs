using System;
using RooseLabs.Input;
using UnityEngine;
using UnityEngine.UI;

namespace RooseLabs.UI
{
    public class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton;

        public Action PlayButtonAction;
        public Action ArchiveButtonAction;
        public Action SettingsButtonAction;
        public Action CreditsButtonAction;
        public Action ExitButtonAction;

        private void OnEnable()
        {
            if (!InputManager.Instance.IsCurrentDeviceKeyboardAndMouse())
                playButton.Select();
        }

        public void PlayButton()
        {
            PlayButtonAction.Invoke();
        }

        public void ArchiveButton()
        {
            ArchiveButtonAction.Invoke();
        }

        public void SettingsButton()
        {
            SettingsButtonAction.Invoke();
        }

        public void CreditsButton()
        {
            CreditsButtonAction.Invoke();
        }

        public void ExitButton()
        {
            ExitButtonAction.Invoke();
        }
    }
}

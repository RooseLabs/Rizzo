using System;
using UnityEngine;

namespace RooseLabs.UI
{
    public class UIMainMenu : MonoBehaviour
    {
        public Action PlayButtonAction;
        public Action ArchiveButtonAction;
        public Action SettingsButtonAction;
        public Action CreditsButtonAction;
        public Action ExitButtonAction;

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

using System;
using RooseLabs.Input;
using UnityEngine;
using UnityEngine.UI;

namespace RooseLabs.UI.Gameplay
{
    public class UIPause : MonoBehaviour
    {
        [SerializeField] private Button resumeButton;

        public event Action ResumeRequested;
        public event Action SettingsScreenRequested;
        public event Action BackToMainRequested;

        private void OnEnable()
        {
            if (!InputManager.Instance.IsCurrentDeviceKeyboardAndMouse())
                resumeButton.Select();
        }

        public void Resume()
        {
            ResumeRequested?.Invoke();
        }

        public void OpenSettingsScreen()
        {
            SettingsScreenRequested?.Invoke();
        }

        public void BackToMainMenu()
        {
            BackToMainRequested?.Invoke();
        }
    }
}

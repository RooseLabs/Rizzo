using System;
using System.Collections.Generic;
using UnityEngine;

namespace RooseLabs.UI.Settings
{
    public enum SettingsType
    {
        Audio,
        Screen
    }

    public class UISettingsManager : MonoBehaviour
    {
        [SerializeField] private UISettingsAudioComponent audioComponent;
        [SerializeField] private UISettingsScreenComponent screenComponent;

        private Dictionary<SettingsType, UISettingsComponent> m_settingsComponents;

        public event Action OnClose = delegate { };

        private void Awake()
        {
            m_settingsComponents = new Dictionary<SettingsType, UISettingsComponent>
            {
                { SettingsType.Audio, audioComponent },
                { SettingsType.Screen, screenComponent }
            };
            if (!Application.isEditor &&
                Application.platform != RuntimePlatform.WindowsPlayer &&
                Application.platform != RuntimePlatform.LinuxPlayer &&
                Application.platform != RuntimePlatform.OSXPlayer)
            {
                // Disable Screen Settings if not on a PC platform
                m_settingsComponents.Remove(SettingsType.Screen);
            }
        }

        private void OnEnable()
        {
            OpenSetting(SettingsType.Screen);
        }

        private void OpenSetting(SettingsType settingsType)
        {
            foreach (var item in m_settingsComponents)
            {
                item.Value.gameObject.SetActive(item.Key == settingsType);
            }
        }

        private void Close() => OnClose.Invoke();
    }
}

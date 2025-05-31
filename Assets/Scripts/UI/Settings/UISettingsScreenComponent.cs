using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RooseLabs.UI.Settings
{
    public class UISettingsScreenComponent : UISettingsComponent
    {
        [SerializeField] private UISettingItemStepper windowModeSetting;
        [SerializeField] private UISettingItemStepper resolutionSetting;
        [SerializeField] private UISettingItemStepper renderScaleSetting;
        [SerializeField] private UISettingItemStepper vsyncSetting;
        [SerializeField] private UISettingItemSlider framerateLimitSetting;

        private FullScreenMode m_windowMode;
        private Resolution m_currentResolution;
        private int m_currentResolutionIndex;
        private List<Resolution> m_resolutionsList;
        private List<Resolution> SupportedResolutions
        {
            get
            {
                return m_resolutionsList ??= Screen.resolutions
                    .GroupBy(res => new { res.width, res.height })
                    .Select(group => group.OrderByDescending(res => res.refreshRateRatio).First())
                    .ToList();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            resolutionSetting.OnNextOption += NextResolution;
            resolutionSetting.OnPreviousOption += PreviousResolution;
            Setup();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            resolutionSetting.OnNextOption -= NextResolution;
            resolutionSetting.OnPreviousOption -= PreviousResolution;
        }

        private void Init()
        {
            m_windowMode = Screen.fullScreenMode;
            m_currentResolution = Screen.currentResolution;
            m_currentResolutionIndex = GetCurrentResolutionIndex();
        }

        private void Setup()
        {
            Init();
            SetResolutionField();
        }

        #region Resolution
        private void SetResolutionField()
        {
            string displayText = SupportedResolutions[m_currentResolutionIndex].ToString();
            displayText = displayText[..displayText.IndexOf("@", StringComparison.Ordinal)].Trim();
            resolutionSetting.DisplayText = displayText;
        }

        private int GetCurrentResolutionIndex()
        {
            return SupportedResolutions.FindIndex(o => o.width == m_currentResolution.width && o.height == m_currentResolution.height);
        }

        private void NextResolution()
        {
            OnResolutionChange(Mathf.Clamp(m_currentResolutionIndex + 1, 0, SupportedResolutions.Count - 1));
        }

        private void PreviousResolution()
        {
            OnResolutionChange(Mathf.Clamp(m_currentResolutionIndex - 1, 0, SupportedResolutions.Count - 1));
        }

        private void OnResolutionChange(int newResolutionIndex)
        {
            if (m_currentResolutionIndex == newResolutionIndex) return;
            m_currentResolutionIndex = newResolutionIndex;
            m_currentResolution = SupportedResolutions[m_currentResolutionIndex];
            SetResolutionField();
        }
        #endregion

        protected override void ApplySettings()
        {
            Screen.SetResolution(m_currentResolution.width, m_currentResolution.height, m_windowMode);
        }

        protected override void ResetSettings()
        {
            Setup();
        }
    }
}

using UnityEngine;

namespace RooseLabs.UI.Settings
{
    public class UISettingsAudioComponent : UISettingsComponent
    {
        [SerializeField] private UISettingItemSlider masterVolumeSetting;
        [SerializeField] private UISettingItemSlider musicVolumeSetting;
        [SerializeField] private UISettingItemSlider sfxVolumeSetting;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void ApplySettings()
        {
            // TODO
        }

        protected override void ResetSettings()
        {
            // TODO
        }
    }
}

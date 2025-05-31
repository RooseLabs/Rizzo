using RooseLabs.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace RooseLabs.UI.Settings
{
    public abstract class UISettingsComponent : MonoBehaviour
    {
        [SerializeField] protected SettingsSO currentSettings;

        [SerializeField] private Button resetButton;
        [SerializeField] private Button applyButton;

        protected virtual void OnEnable()
        {
            resetButton.onClick.AddListener(ResetSettings);
            applyButton.onClick.AddListener(ApplySettings);
        }

        protected virtual void OnDisable()
        {
            resetButton.onClick.RemoveAllListeners();
            applyButton.onClick.RemoveAllListeners();
        }

        protected abstract void ApplySettings();
        protected abstract void ResetSettings();
    }
}

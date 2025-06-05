using RooseLabs.Events.Channels;
using UnityEngine;
using UnityEngine.UI;

namespace RooseLabs.UI.Gameplay
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] private Slider healthDisplaySlider;

        [Header("Listening to")]
        [SerializeField] private FloatEventChannelSO onPlayerHealthChanged;

        private void OnEnable()
        {
            onPlayerHealthChanged.OnEventRaised += OnPlayerHealthChanged;
        }

        private void OnDisable()
        {
            onPlayerHealthChanged.OnEventRaised -= OnPlayerHealthChanged;
        }

        private void OnPlayerHealthChanged(float value)
        {
            healthDisplaySlider.value = value;
        }
    }
}

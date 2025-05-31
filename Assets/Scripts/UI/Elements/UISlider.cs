using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RooseLabs.UI.Elements
{
    public class UISlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text sliderValueText;

        public Action<float> OnValueChanged;

        private void OnEnable()
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnDisable()
        {
            slider.onValueChanged.RemoveAllListeners();
        }

        private void Start()
        {
            SetValueText(slider.value);
        }

        private void OnSliderValueChanged(float value)
        {
            SetValueText(value);
            OnValueChanged?.Invoke(value);
        }

        private void SetValueText(float value)
        {
            sliderValueText.text = Mathf.CeilToInt(value).ToString();
        }
    }
}

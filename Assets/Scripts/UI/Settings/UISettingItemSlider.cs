using System;
using RooseLabs.UI.Elements;
using UnityEngine;

namespace RooseLabs.UI.Settings
{
    public class UISettingItemSlider : MonoBehaviour
    {
        [SerializeField] private UISlider slider;

        public event Action<float> OnValueChanged
        {
            add => slider.OnValueChanged += value;
            remove => slider.OnValueChanged -= value;
        }
    }
}

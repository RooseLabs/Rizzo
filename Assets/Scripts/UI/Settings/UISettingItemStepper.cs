using System;
using RooseLabs.UI.Elements;
using UnityEngine;

namespace RooseLabs.UI.Settings
{
    public class UISettingItemStepper : MonoBehaviour
    {
        [SerializeField] private UIStepper stepper;

        public event Action OnNextOption
        {
            add => stepper.OnNextOption += value;
            remove => stepper.OnNextOption -= value;
        }

        public event Action OnPreviousOption
        {
            add => stepper.OnPreviousOption += value;
            remove => stepper.OnPreviousOption -= value;
        }

        public string DisplayText
        {
            get => stepper.DisplayText;
            set => stepper.DisplayText = value;
        }
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RooseLabs.UI.Elements
{
    public class UIStepper : MonoBehaviour
    {
        [SerializeField] private Button prevButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private TMP_Text selectedOptionText;

        public event Action OnNextOption = delegate { };
        public event Action OnPreviousOption = delegate { };

        private void OnEnable()
        {
            prevButton.onClick.AddListener(PreviousOption);
            nextButton.onClick.AddListener(NextOption);
        }

        private void OnDisable()
        {
            prevButton.onClick.RemoveAllListeners();
            nextButton.onClick.RemoveAllListeners();
        }

        private void PreviousOption()
        {
            OnPreviousOption?.Invoke();
        }

        private void NextOption()
        {
            OnNextOption?.Invoke();
        }

        public string DisplayText
        {
            get => selectedOptionText.text;
            set => selectedOptionText.text = value;
        }
    }
}

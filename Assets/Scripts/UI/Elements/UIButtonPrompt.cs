using RooseLabs.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RooseLabs.UI.Elements
{
    public class UIButtonPrompt : MonoBehaviour
    {
        [SerializeField] private string actionName;

        [Header("Button Prompt Images")]
        [SerializeField] private Sprite kbmButton;
        [SerializeField] private Sprite gamepadButton;

        // [SerializeField] private InputAction inputAction;
        // TODO: Leverage the InputAction system to get the current binding and show the correct button prompt instead of hardcoding it

        private Image m_buttonImage;
        private TextMeshProUGUI m_buttonText;

        private InputManager m_inputManager;

        private void Awake()
        {
            m_inputManager = InputManager.Instance;
            m_buttonImage = GetComponentInChildren<Image>();
            m_buttonText = GetComponentInChildren<TextMeshProUGUI>();
            m_buttonText.text = actionName;
        }

        private void OnEnable()
        {
            m_inputManager.InputDeviceChangedEvent += UpdateButtonPrompt;
            UpdateButtonPrompt(InputManager.Instance.CurrentDevice);
        }

        private void OnDisable()
        {
            m_inputManager.InputDeviceChangedEvent -= UpdateButtonPrompt;
        }

        private void UpdateButtonPrompt(InputDevice device)
        {
            m_buttonImage.sprite = device switch
            {
                Keyboard or Mouse => kbmButton,
                Gamepad => gamepadButton,
                _ => m_buttonImage.sprite
            };
        }
    }
}

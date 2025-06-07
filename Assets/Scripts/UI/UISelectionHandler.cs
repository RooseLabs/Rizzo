using System.Linq;
using RooseLabs.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RooseLabs.UI
{
    public class UISelectionHandler : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private Selectable defaultSelection;
        private GameObject m_lastSelectedGameObject;

        private void OnEnable()
        {
            var select = m_lastSelectedGameObject ?? defaultSelection?.gameObject;
            if (select is not null && !InputManager.Instance.IsCurrentDeviceKeyboardAndMouse())
                EventSystem.current?.SetSelectedGameObject(select);
            InputManager.Instance.InputDeviceChangedEvent += OnInputDeviceChanged;
        }

        private void OnDisable()
        {
            InputManager.Instance.InputDeviceChangedEvent -= OnInputDeviceChanged;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (EventSystem.current.alreadySelecting) return;
            var selectable = eventData.hovered
                .Select(x => x.GetComponent<Selectable>())
                .FirstOrDefault(x => x != null);
            if (selectable == null) return;
            EventSystem.current.SetSelectedGameObject(selectable.gameObject);
            m_lastSelectedGameObject = selectable.gameObject;
            selectable.OnPointerExit(eventData);
        }

        private void OnInputDeviceChanged(InputDevice device)
        {
            if (device is not Gamepad) return;
            var select = m_lastSelectedGameObject ?? defaultSelection?.gameObject;
            if (select is not null) EventSystem.current.SetSelectedGameObject(select);
        }
    }
}

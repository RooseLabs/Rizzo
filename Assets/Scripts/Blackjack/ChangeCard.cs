using UnityEngine;
using UnityEngine.UI;

namespace RooseLabs.Blackjack
{
    public class ChangeCard : MonoBehaviour
    {
        [SerializeField] private GameObject card;
        [SerializeField] private Button hitButton;
        private int m_cardIndex;
        private CardsBehaviour m_cards;

        private void Awake()
        {
            m_cards = card.GetComponent<CardsBehaviour>();
        }

        private void Start()
        {
            if (hitButton != null) hitButton.onClick.AddListener(OnHitButtonClick);
        }

        private void OnHitButtonClick()
        {
            if (m_cardIndex >= m_cards.CardFaces.Length)
            {
                // Reset the index if it exceeds the array length
                m_cardIndex = 0;
                m_cards.ToggleCardFace(false);
                card.SetActive(false); // Deactivate the card
            }
            else
            {
                card.SetActive(true); // Ensure the card is active
                m_cards.CardIndex = m_cardIndex;
                m_cards.ToggleCardFace(true);
                m_cardIndex++;
            }
        }
    }
}

using UnityEngine;

namespace RooseLabs.Blackjack
{
    public class CardsBehaviour : MonoBehaviour
    {
        [SerializeField] private Sprite[] cardFaces;
        [SerializeField] private Sprite cardBack;
        [SerializeField] private int cardIndex;
        private SpriteRenderer m_spriteRenderer;

        private void Awake()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void ToggleCardFace(bool showFace)
        {
            m_spriteRenderer.sprite = showFace ? cardFaces[cardIndex] : cardBack;
        }

        public Sprite[] CardFaces => cardFaces;

        public int CardIndex
        {
            get => cardIndex;
            set
            {
                if (value >= 0 && value < cardFaces.Length)
                {
                    cardIndex = value;
                    m_spriteRenderer.sprite = cardFaces[cardIndex];
                }
                else
                {
                    Debug.LogError("Invalid card index");
                }
            }
        }
    }
}

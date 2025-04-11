using System.Collections.Generic;
using UnityEngine;

namespace RooseLabs.Blackjack
{
    [RequireComponent(typeof(CardStack))]
    public class CardStackView : MonoBehaviour
    {
        [SerializeField] private Vector3 start;
        [SerializeField] private float cardOffset;
        [SerializeField] private bool faceUp;
        [SerializeField] private bool cardDeckOrder;
        [SerializeField] private GameObject cardPrefab;

        private CardStack m_cardStack;
        private Dictionary<int, CardView> m_fetchedCards;

        private void Awake()
        {
            m_fetchedCards = new Dictionary<int, CardView>();
            m_cardStack = GetComponent<CardStack>();
            ShowCards();

            m_cardStack.CardRemoved += OnCardRemoved;
            m_cardStack.CardAdded += OnCardAdded;
        }

        private void Update()
        {
            ShowCards();
        }

        public void ShowCards()
        {
            var cardCount = 0;

            if (m_cardStack.HasCards)
                foreach (var i in m_cardStack.GetCards())
                {
                    var co = cardOffset * cardCount;
                    var temp = start + new Vector3(co, 0f);
                    AddCard(temp, i, cardCount);
                    cardCount++;
                }
        }

        private void AddCard(Vector3 position, int cardIndex, int positionalIndex)
        {
            if (m_fetchedCards.ContainsKey(cardIndex))
            {
                if (!faceUp)
                {
                    var behavior = m_fetchedCards[cardIndex].Card.GetComponent<CardsBehavior>();
                    behavior.ToggleCardFace(m_fetchedCards[cardIndex].IsFaceUp);
                }

                return;
            }

            var cardCopy = Instantiate(cardPrefab);
            cardCopy.transform.position = position;
            var cardsBehavior = cardCopy.GetComponent<CardsBehavior>();
            cardsBehavior.CardIndex = cardIndex;
            cardsBehavior.ToggleCardFace(faceUp);

            var spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();

            if (cardDeckOrder)
                spriteRenderer.sortingOrder = 51 - positionalIndex;
            else
                spriteRenderer.sortingOrder = positionalIndex;

            m_fetchedCards.Add(cardIndex, new CardView(cardCopy));
        }

        private void OnCardAdded(object sender, CardEventArgs e)
        {
            var co = cardOffset * m_cardStack.CardCount;
            var temp = start + new Vector3(co, 0f);
            AddCard(temp, e.CardIndex, m_cardStack.CardCount);
        }

        private void OnCardRemoved(object sender, CardEventArgs e)
        {
            if (m_fetchedCards.ContainsKey(e.CardIndex))
            {
                Destroy(m_fetchedCards[e.CardIndex].Card);
                m_fetchedCards.Remove(e.CardIndex);
            }
        }

        public void Clear()
        {
            m_cardStack.Reset();
            foreach (CardView view in m_fetchedCards.Values)
                Destroy(view.Card);
            m_fetchedCards.Clear();
        }

        public void Toogle(int card, bool isFaceUp)
        {
            m_fetchedCards[card].IsFaceUp = isFaceUp;
        }
    }
}

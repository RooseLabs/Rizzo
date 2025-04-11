using System.Collections.Generic;
using RooseLabs.Utils;
using UnityEngine;

namespace RooseLabs.Blackjack
{
    public class CardStack : MonoBehaviour
    {
        [SerializeField] private bool isGameDeck;

        private List<int> m_cards;

        public bool HasCards => m_cards.Count > 0;
        public int CardCount => m_cards?.Count ?? 0;

        private void Awake()
        {
            m_cards = new List<int>();
            if (isGameDeck) CreateDeck();
        }

        public void Reset()
        {
            m_cards.Clear();
        }

        public event CardEventHandler CardRemoved;
        public event CardEventHandler CardAdded;

        public int Pop()
        {
            var temp = m_cards[0];
            m_cards.RemoveAt(0);

            if (CardRemoved != null) CardRemoved(this, new CardEventArgs(temp));
            return temp;
        }

        public void Push(int card)
        {
            m_cards.Add(card);
            if (CardAdded != null) CardAdded(this, new CardEventArgs(card));
        }

        public IEnumerable<int> GetCards()
        {
            foreach (int card in m_cards)
            {
                yield return card;
            }
        }

        public void CreateDeck()
        {
            m_cards.Clear();

            for (int i = 0; i < 52; i++)
            {
                m_cards.Add(i);
            }

            m_cards.Shuffle();
        }

        public bool HasCard(int cardId)
        {
            return m_cards.FindIndex(i => i == cardId) >= 0;
        }

        public int HandValue()
        {
            int handValue = 0;
            int aces = 0;

            foreach (int card in GetCards())
            {
                int value = card % 13;
                if (value <= 8)
                {
                    value += 2;
                }
                else if (value is >= 8 and < 12)
                {
                    value = 10;
                    handValue += value;
                }
                else
                {
                    aces++;
                }
            }

            for (var i = 0; i < aces; i++)
            {
                if (handValue + 11 <= 21)
                {
                    handValue += 11;
                }
                else
                {
                    handValue++;
                }
            }

            return handValue;
        }
    }
}

using UnityEngine;

namespace RooseLabs.Blackjack
{
    public class CardView
    {
        public CardView(GameObject card)
        {
            Card = card;
            IsFaceUp = false;
        }

        public GameObject Card { get; private set; }
        public bool IsFaceUp { get; set; }
    }
}

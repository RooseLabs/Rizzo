using System;

namespace RooseLabs.Blackjack
{
    public delegate void CardEventHandler(object sender, CardEventArgs e);

    public class CardEventArgs : EventArgs
    {
        public CardEventArgs(int cardIndex)
        {
            CardIndex = cardIndex;
        }

        public int CardIndex { get; private set; }
    }
}

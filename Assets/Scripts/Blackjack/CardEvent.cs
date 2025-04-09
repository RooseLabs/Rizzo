using System;

public delegate void CardEventHandler(object sender, CardEventArgs e);

public class CardEventArgs : EventArgs
{
    public int CardIndex { get; private set; }
    
    public CardEventArgs(int cardIndex)
    {
        CardIndex = cardIndex;
    }
}

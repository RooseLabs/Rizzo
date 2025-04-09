using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardStack : MonoBehaviour
{
    List<int> _cards;
    
    public bool isGameDeck; 
    public bool HasCards => _cards is { Count: > 0 };

    public event CardEventHandler CardRemoved;
    public event CardEventHandler CardAdded;
    
    void Awake()
    {
        _cards = new List<int>();
        if (isGameDeck)
        {
            CreateDeck();
        }
    }

    public void Reset() { _cards.Clear(); }

    public int Pop()
    {
        int temp = _cards[0];
        _cards.RemoveAt(0);
        
        if (CardRemoved != null)
        {
            CardRemoved(this, new CardEventArgs(temp));
        }
        return temp;
    }

    public void Push(int card)
    {
        _cards.Add(card);
        if (CardAdded != null)
        {
            CardAdded(this, new CardEventArgs(card));
        }
    }
    
    public int CardCount
    {
        get
        {
            if (_cards == null)
            {
                return 0;
            }
            else
            {
                return _cards.Count;
            }
        }
    }
    
    public IEnumerable<int> GetCards()
    {
        foreach (int card in _cards)
        {
            yield return card;
        }
    }
    
    public void CreateDeck()
    {
        _cards.Clear();
        
        for (int i = 0; i < 52; i++)
        {
            _cards.Add(i);
        }
        
        int n = _cards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (_cards[k], _cards[n]) = (_cards[n], _cards[k]);
            /* int value = _cards[k];
            _cards[k] = _cards[n];
            _cards[n] = value;*/
        }
    }
    
    public bool HasCard(int cardId)
    {
        return _cards.FindIndex(i => i == cardId) >= 0;
    }
    
    public bool HasCardOld(int cardId)
    {
        foreach (int i in _cards)
        {
            if (i == cardId) return true;
        }
        return false;
    }

    public int HandValue()
    {//
        int _handValue = 0;
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
                value= 10;
                _handValue += value;
            }
            else
            {
                aces++;
            }
          
        }
        for(int i = 0; i < aces; i++)
        {
            if (_handValue +11 <= 21)
            {
                _handValue += 11;
            }
            else
            {
                _handValue += 1;
            }
        }
        return _handValue;
    }
}

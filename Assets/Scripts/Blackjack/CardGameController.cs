using System;
using UnityEngine;
using UnityEngine.UI;

public class CardGameController : MonoBehaviour
{
    int _dealerfirstCard;
    
    public CardStack dealer;
    public CardStack player;
    public CardStack deck;

    public Button hitButton;
    public Button standButton;
    
    private void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            player.Push(deck.Pop());
            dealer.Push(deck.Pop());
        }
    }

    public void OnHitButtonClick()
    {
        player.Push(deck.Pop());
        Debug.Log("Player Hand Value: " + player.HandValue());
        if (player.HandValue() > 21)
        {
            hitButton.interactable = false;
            standButton.interactable = false;
        }
    }
    public void OnStandButtonClick()
    {
        hitButton.interactable = false;
        standButton.interactable = false;
        dealerTurn();
    }
    
    /*void onHit()
    {
        int card = deck.Pop();
        dealer.Push(card);
        if (dealer.CardCount >=2)
        {
           CardStackView dealerView = dealer.GetComponent<CardStackView>();
        }
    }*/
    
    void HitDealer()
    {
        int card = deck.Pop();

        if (_dealerfirstCard < 0)
        {
            _dealerfirstCard = card;
        }

        dealer.Push(card);
        if (dealer.CardCount >= 2)
        {
            CardStackView view = dealer.GetComponent<CardStackView>();
            view.Toogle(card, true);
        }
    }

    
    
    
    void dealerTurn() //donFelix Ai 
    {
        while (dealer.HandValue() < 17)
        {
            CardStackView view = dealer.GetComponent<CardStackView>();
            view.Toogle(_dealerfirstCard, true);
            view.ShowCards();

            while (dealer.HandValue() < 17)
            {
                HitDealer();
            }
            
            if (dealer.HandValue() > 21 || (dealer.HandValue() >= player.HandValue() && dealer.HandValue() <= 21))
            {
                Debug.Log("lose");
                break;
            }
            else if (dealer.HandValue() >21 || (player.HandValue() <= 21 && player.HandValue() > dealer.HandValue()))
            {
                Debug.Log("winner");
                break;
            }
            else
            {
               Debug.Log("The house wins!"); 
                break;
            }
           
        }
    }
}


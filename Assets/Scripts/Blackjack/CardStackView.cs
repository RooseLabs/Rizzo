using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CardStack))]
public class CardStackView : MonoBehaviour
{
   CardStack _cardStack;
   Dictionary<int, CardView> _fetchedCards;
   
   public Vector3 start;
   public float cardOffset;
   public bool faceUp =false;
   public bool CardDeckOrder=false;
   public GameObject cardPrefab;
   

   private void Awake()
   {
      _fetchedCards = new Dictionary<int, CardView>();
      _cardStack = GetComponent<CardStack>();
      ShowCards();
      
      _cardStack.CardRemoved += OnCardRemoved;
      _cardStack.CardAdded += OnCardAdded;
   }
   
   private void Update()
   {
      ShowCards();
   }
   
   public void ShowCards()
   {
      int cardCount = 0;

      if (_cardStack.HasCards)
      {
         foreach (int i in _cardStack.GetCards())
         {
            float co = cardOffset * cardCount;
            Vector3 temp = start + new Vector3(co, 0f);
            AddCard(temp, i, cardCount);
            cardCount++;
         }
      }
   }
   
   void AddCard(Vector3 position, int cardIndex, int positionalIndex)
   {
      if (_fetchedCards.ContainsKey(cardIndex))
      {
         if (!faceUp)
         {
            CardsBehavior behavior = _fetchedCards[cardIndex].Card.GetComponent<CardsBehavior>();
            behavior.ToggleCardFace(_fetchedCards[cardIndex].isFaceUp);
         }
         return;
      }

      GameObject cardCopy = (GameObject)Instantiate(cardPrefab);
      cardCopy.transform.position = position;
      CardsBehavior cardsBehavior = cardCopy.GetComponent<CardsBehavior>();
      cardsBehavior.cardIndex = cardIndex;
      
      if(faceUp)
      {
         cardsBehavior.ToggleCardFace(faceUp);
      }
      else
      {
         cardsBehavior.ToggleCardFace(faceUp);
      }

      SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();
     
      if (CardDeckOrder)
      {
         spriteRenderer.sortingOrder= 51 - positionalIndex;
      }
      else
      {
         spriteRenderer.sortingOrder = positionalIndex;
      }
      
      _fetchedCards.Add(cardIndex, new CardView(cardCopy));
   }
   
   private void OnCardAdded(object sender, CardEventArgs e)
   {
      float co = cardOffset * _cardStack.CardCount;
      Vector3 temp = start + new Vector3(co, 0f);
      AddCard(temp, e.CardIndex, _cardStack.CardCount);
   }
   
   private void OnCardRemoved(object sender, CardEventArgs e)
   {
      if (_fetchedCards.ContainsKey(e.CardIndex))
      {
         Destroy(_fetchedCards[e.CardIndex].Card);
         _fetchedCards.Remove(e.CardIndex);
      }
   }

   public void Clear()
   {
      _cardStack.Reset();
      foreach(CardView view in _fetchedCards.Values)
      {
         Destroy(view.Card);
      }
      _fetchedCards.Clear();
   }

   public void Toogle(int card, bool isFaceUp)
   {
      _fetchedCards[card].isFaceUp = isFaceUp;
   }

}


using UnityEngine;
using UnityEngine.UI;

namespace RooseLabs.Blackjack
{
    public class CardGameController : MonoBehaviour
    {
        [SerializeField] private CardStack dealer;
        [SerializeField] private CardStack player;
        [SerializeField] private CardStack deck;
        [SerializeField] private Button hitButton;
        [SerializeField] private Button standButton;

        private int m_dealerFirstCard;

        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            for (var i = 0; i < 2; i++)
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

        private void HitDealer()
        {
            var card = deck.Pop();

            if (m_dealerFirstCard < 0) m_dealerFirstCard = card;

            dealer.Push(card);
            if (dealer.CardCount >= 2)
            {
                var view = dealer.GetComponent<CardStackView>();
                view.Toogle(card, true);
            }
        }

        private void dealerTurn() //donFelix Ai
        {
            while (dealer.HandValue() < 17)
            {
                var view = dealer.GetComponent<CardStackView>();
                view.Toogle(m_dealerFirstCard, true);
                view.ShowCards();

                while (dealer.HandValue() < 17) HitDealer();

                if (dealer.HandValue() > 21 || (dealer.HandValue() >= player.HandValue() && dealer.HandValue() <= 21))
                {
                    Debug.Log("lose");
                    break;
                }

                if (dealer.HandValue() > 21 || (player.HandValue() <= 21 && player.HandValue() > dealer.HandValue()))
                {
                    Debug.Log("winner");
                    break;
                }

                Debug.Log("The house wins!");
                break;
            }
        }
    }
}

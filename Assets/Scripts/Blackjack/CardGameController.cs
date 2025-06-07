using System.Collections;
using System.Collections.Generic;
using RooseLabs.Events.Channels;
using RooseLabs.Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RooseLabs.Blackjack
{
    public class CardGameController : MonoBehaviour
    {
        [Header("Card Game Settings")]
        [SerializeField] private CardStack dealer;
        [SerializeField] private CardStack player;
        [SerializeField] private CardStack deck;
        [SerializeField] private Button hitButton;
        [SerializeField] private Button standButton;
        [SerializeField] private int defaultDamage;
        [SerializeField] private float cooldownTime;

        [Header("Player and Dealer Hand Values")]
        [SerializeField] private TMP_Text playerHandValueText;
        [SerializeField] private TMP_Text dealerHandValueText;

        [Header("Health Images")]
        [SerializeField] private Image playerHealthImage;
        [SerializeField] private Image dealerHealthImage;

        [Header("Player Health Sprites")]
        [SerializeField] private Sprite playerHealth100Sprite;
        [SerializeField] private Sprite playerHealth50Sprite;
        [SerializeField] private Sprite playerHealth25Sprite;
        [SerializeField] private Sprite playerHealth0Sprite;

        [Header("Dealer Health Sprites")]
        [SerializeField] private Sprite dealerHealth100Sprite;
        [SerializeField] private Sprite dealerHealth50Sprite;
        [SerializeField] private Sprite dealerHealth25Sprite;
        [SerializeField] private Sprite dealerHealth0Sprite;

        [Header("Broadcasting on")]
        [SerializeField] private BoolEventChannelSO hudVisibilityChannel;

        private int m_dealerFirstCard = -1;

        private void Start()
        {
            hudVisibilityChannel.RaiseEvent(false);
            InputManager.Instance.EnableGameplayInput();
            StartGame();
        }

        private void StartGame()
        {
            Debug.Log("Rizzo Health: " + player.HealthPoints);
            Debug.Log("Don Felix Health: " + dealer.HealthPoints);

            // Ensure deck is filled before dealing
            if (deck.CardCount < 4)
            {
                deck.Reset();
                deck.CreateDeck();
            }

            for (var i = 0; i < 2; i++)
            {
                var playerCard = deck.Pop();
                if (playerCard >= 0) player.Push(playerCard);

                var dealerCard = deck.Pop();
                if (dealerCard >= 0) dealer.Push(dealerCard);
            }
            UpdateHandValueTexts();
        }

        private void ResetGame()
        {
            // Move all cards from player and dealer back to the deck, removing them properly
            var playerCards = new List<int>(player.GetCards());
            foreach (var card in playerCards)
            {
                player.RemoveCard(card);
                deck.Push(card);
            }

            var dealerCards = new List<int>(dealer.GetCards());
            foreach (var card in dealerCards)
            {
                dealer.RemoveCard(card);
                deck.Push(card);
            }

            player.Reset();
            dealer.Reset();

            m_dealerFirstCard = -1; // Reset dealer's first card
            UpdateHandValueTexts();

            // Re-enable buttons for the new round
            hitButton.interactable = true;
            standButton.interactable = true;

            // Check for game over before starting a new round
            if (player.HealthPoints <= 0)
            {
                Debug.Log("Game Over! Rizzo has no health left.");
                hitButton.interactable = false;
                standButton.interactable = false;
                // Optionally, trigger game over UI or logic here
                return;
            }

            if (dealer.HealthPoints <= 0)
            {
                Debug.Log("Congratulations! Don Felix has no health left.");
                hitButton.interactable = false;
                standButton.interactable = false;
                // Optionally, trigger victory UI or logic here
                return;
            }

            StartGame(); // Deals 2 cards to each hand and logs health
        }

        // Utility coroutine for cooldowns
        private IEnumerator WaitForSecondsCoroutine(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }

        public void OnHitButtonClick()
        {
            hitButton.interactable = false;
            standButton.interactable = false;
            StartCoroutine(PlayerHitWithCooldown());
        }

        private IEnumerator PlayerHitWithCooldown()
        {
            player.Push(deck.Pop());
            Debug.Log("Player Hand Value: " + player.GetHandValue());
            UpdateHandValueTexts();
            yield return StartCoroutine(WaitForSecondsCoroutine(cooldownTime)); // Use cooldownTime

            var playerHand = player.GetHandValue();

            if (playerHand >= 21)
            {
                Debug.Log(playerHand == 21 ? "Rizzo hits Blackjack!" : "Rizzo busts!");
                StartCoroutine(DealerTurnCoroutine());
            }
            else
            {
                hitButton.interactable = true;
                standButton.interactable = true;
            }
        }

        public void OnStandButtonClick()
        {
            hitButton.interactable = false;
            standButton.interactable = false;
            StartCoroutine(DealerTurnCoroutine());
        }

        // Dealer's turn with cooldowns
        private IEnumerator DealerTurnCoroutine()
        {
            var view = dealer.GetComponent<CardStackView>();
            if (m_dealerFirstCard >= 0) view.Toogle(m_dealerFirstCard, true);
            view.ShowCards();

            var dealerTarget = Random.Range(17, 21); // upper bound is exclusive

            while (dealer.GetHandValue() < dealerTarget) yield return StartCoroutine(DealerHitWithCooldown());

            var playerHand = player.GetHandValue();
            var dealerHand = dealer.GetHandValue();

            yield return StartCoroutine(WaitForSecondsCoroutine(cooldownTime)); // Use cooldownTime
            ApplyDamageRules(playerHand, dealerHand);
        }

        private IEnumerator DealerHitWithCooldown()
        {
            HitDealer();
            float thinkTime = Random.Range(cooldownTime, cooldownTime * 5.0f); // Use cooldownTime as base
            yield return StartCoroutine(WaitForSecondsCoroutine(thinkTime));
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
            UpdateHandValueTexts();
        }

        //function for damage rules
        private void ApplyDamageRules(int playerHand, int dealerHand)
        {
            var playerBlackjack = playerHand == 21;
            var dealerBlackjack = dealerHand == 21;
            var playerBust = playerHand > 21;
            var dealerBust = dealerHand > 21;

            var critRange = Random.Range(2, 5);
            var highCritRange = Random.Range(3, 7);

            if (playerBust && dealerBlackjack)
            {
                player.HealthPoints -= defaultDamage * highCritRange;
                Debug.Log("Rizzo busts against Don Felix's Blackjack! Health Points: " + player.HealthPoints);
                UpdateHealthImage(true);
                UpdateHealthImage(false);
                StartCoroutine(ResetGameWithCooldown(cooldownTime));
                return;
            }

            if (dealerBust && playerBlackjack)
            {
                dealer.HealthPoints -= defaultDamage * highCritRange;
                Debug.Log("Don Felix busts against Rizzo's Blackjack! Health Points: " + dealer.HealthPoints);
                UpdateHealthImage(true);
                UpdateHealthImage(false);
                StartCoroutine(ResetGameWithCooldown(cooldownTime));
                return;
            }

            if (playerBlackjack && !dealerBlackjack)
            {
                Debug.Log("Rizzo hits Blackjack! No damage taken.");
                dealer.HealthPoints -= defaultDamage * critRange;
                Debug.Log("Don Felix HP: " + dealer.HealthPoints);
                UpdateHealthImage(true);
                UpdateHealthImage(false);
                StartCoroutine(ResetGameWithCooldown(cooldownTime));
                return;
            }

            if (dealerBlackjack && !playerBlackjack)
            {
                Debug.Log("Don Felix hits Blackjack! No damage taken.");
                player.HealthPoints -= defaultDamage * critRange;
                Debug.Log("Rizzo HP: " + player.HealthPoints);
                UpdateHealthImage(true);
                UpdateHealthImage(false);
                StartCoroutine(ResetGameWithCooldown(cooldownTime));
                return;
            }

            if (playerBlackjack && dealerBlackjack)
            {
                Debug.Log("Both hit Blackjack! No damage taken.");
                UpdateHealthImage(true);
                UpdateHealthImage(false);
                StartCoroutine(ResetGameWithCooldown(cooldownTime));
                return;
            }

            if (playerBust && dealerBust)
            {
                player.HealthPoints -= defaultDamage * critRange;
                dealer.HealthPoints -= defaultDamage * critRange;
                Debug.Log("Both bust! Rizzo HP: " + player.HealthPoints + ", Don Felix HP: " + dealer.HealthPoints);
                UpdateHealthImage(true);
                UpdateHealthImage(false);
                StartCoroutine(ResetGameWithCooldown(cooldownTime));
                return;
            }

            if (playerBust && !dealerBust)
            {
                player.HealthPoints -= defaultDamage;
                Debug.Log("Rizzo busts! Health Points: " + player.HealthPoints);
                UpdateHealthImage(true);
                UpdateHealthImage(false);
                StartCoroutine(ResetGameWithCooldown(cooldownTime));
                return;
            }

            if (dealerBust && !playerBust)
            {
                dealer.HealthPoints -= defaultDamage;
                Debug.Log("Don Felix busts! Health Points: " + dealer.HealthPoints);
                UpdateHealthImage(true);
                UpdateHealthImage(false);
                StartCoroutine(ResetGameWithCooldown(cooldownTime));
                return;
            }

            if (playerHand > dealerHand)
            {
                var damage = playerHand - dealerHand;
                if (damage == 0) damage = defaultDamage;
                dealer.HealthPoints -= damage;
                Debug.Log($"Rizzo wins! Don Felix takes {damage} damage. HP: {dealer.HealthPoints}");
            }
            else if (dealerHand > playerHand)
            {
                var damage = dealerHand - playerHand;
                if (damage == 0) damage = defaultDamage;
                player.HealthPoints -= damage;
                Debug.Log($"Don Felix wins! Rizzo takes {damage} damage. HP: {player.HealthPoints}");
            }
            else
            {
                Debug.Log("Tie! No damage taken.");
            }

            UpdateHealthImage(true);
            UpdateHealthImage(false);
            StartCoroutine(ResetGameWithCooldown(cooldownTime));
        }

        // Coroutine for reset cooldown
        private IEnumerator ResetGameWithCooldown(float seconds)
        {
            yield return StartCoroutine(WaitForSecondsCoroutine(seconds * 2.0f)); 
            ResetGame();
        }

        private void UpdateHealthImage(bool isPlayer)
        {
            float health = Mathf.Max(0, isPlayer ? player.HealthPoints : dealer.HealthPoints);
            float maxHealth = isPlayer ? 100f : 200f;
            Image img = isPlayer ? playerHealthImage : dealerHealthImage;

            float percent = health / maxHealth;

            if (isPlayer)
            {
                if (percent <= 0f)
                    img.sprite = playerHealth0Sprite;
                else if (percent <= 0.25f)
                    img.sprite = playerHealth25Sprite;
                else if (percent <= 0.5f)
                    img.sprite = playerHealth50Sprite;
                else
                    img.sprite = playerHealth100Sprite;
            }
            else
            {
                if (percent <= 0f)
                    img.sprite = dealerHealth0Sprite;
                else if (percent <= 0.25f)
                    img.sprite = dealerHealth25Sprite;
                else if (percent <= 0.5f)
                    img.sprite = dealerHealth50Sprite;
                else
                    img.sprite = dealerHealth100Sprite;
            }
        }

        private void UpdateHandValueTexts()
        {
            playerHandValueText.text = player.GetHandValue().ToString();
            dealerHandValueText.text = dealer.GetHandValue().ToString();
        }
    }
}

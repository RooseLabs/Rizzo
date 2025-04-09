using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCard : MonoBehaviour
{
    private CardsBehavior _cards;
    private int _cardIndex;
    private Button _hitButton;

    public GameObject card;
    
    void Awake()
    {
        _cards = card.GetComponent<CardsBehavior>();
        _hitButton = GameObject.Find("Hit").GetComponent<Button>();

    }
    void Start()
    {
        if (_hitButton != null)
        {
            _hitButton.onClick.AddListener(OnHitButtonClick);
        }
    }
    
    private void OnHitButtonClick()
    {
        // Handle hit button click event
        Debug.Log("Hit button clicked!");
        if (_cardIndex >= _cards.cardFaces.Length)
        {
            // Reset the index if it exceeds the array length
            _cardIndex = 0;
            _cards.ToggleCardFace(false);
            card.SetActive(false); // Deactivate the card
        }
        else
        {
            card.SetActive(true); // Ensure the card is active
            _cards.cardIndex = _cardIndex;
            _cards.ToggleCardFace(true);
            _cardIndex++;
        }
    }
}

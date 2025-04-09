using System;
using System.Collections.Generic;
using UnityEngine;

public class CardsBehavior : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    public Sprite[] cardFaces;
    public Sprite cardBack;
    public int cardIndex; // cardFaces[cardIndex];
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void ToggleCardFace(bool showFace)
    {
        _spriteRenderer.sprite = showFace ? cardFaces[cardIndex] : // Show the card face
            cardBack; // Show the card back
    }
}

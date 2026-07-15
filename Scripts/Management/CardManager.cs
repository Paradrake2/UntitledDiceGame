using System;
using UnityEngine;

// Manages the player's collection of unlocked cards.
public class CardManager : MonoBehaviour
{
    public static event Action<Card> OnCardUnlocked; // Event triggered when a card is unlocked.
    void OnEnable()
    {
        OnCardUnlocked += UnlockCard;
    }
    void OnDisable()
    {
        OnCardUnlocked -= UnlockCard;
    }

    public static CardManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public Card[] unlockedCards; // Array of cards that the player has unlocked.
    public Card[] defaultCards;
    public Card[] AllCards; // Array of all available cards in the game.
    public void UnlockCard(Card card)
    {
        // Check if the card is already unlocked
        foreach (var unlockedCard in unlockedCards)
        {
            if (unlockedCard == card)
            {
                Debug.Log("Card already unlocked: " + card.name);
                return; // Card is already unlocked, exit the method
            }
        }

        // Add the card to the unlocked cards array
        int newSize = unlockedCards.Length + 1;
        Card[] newUnlockedCards = new Card[newSize];
        for (int i = 0; i < unlockedCards.Length; i++)
        {
            newUnlockedCards[i] = unlockedCards[i];
        }
        newUnlockedCards[newSize - 1] = card;
        unlockedCards = newUnlockedCards;

        Debug.Log("Card unlocked: " + card.name);
    }
}

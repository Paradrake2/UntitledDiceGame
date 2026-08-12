using System;
using UnityEngine;

// Manages the six card slots and plays cards during a battle.
public class BattleCardManager : MonoBehaviour
{
    public static BattleCardManager Instance { get; private set; }

    [SerializeField] private Card pos1Card;
    [SerializeField] private Card pos2Card;
    [SerializeField] private Card pos3Card;
    [SerializeField] private Card pos4Card;
    [SerializeField] private Card pos5Card;
    [SerializeField] private Card pos6Card;

    [SerializeField] private CardUI pos1CardUI;
    [SerializeField] private CardUI pos2CardUI;
    [SerializeField] private CardUI pos3CardUI;
    [SerializeField] private CardUI pos4CardUI;
    [SerializeField] private CardUI pos5CardUI;
    [SerializeField] private CardUI pos6CardUI;
    public Card[] runCards; // array to hold all the cards the player has this run
    public event Action StartUp;
    
    void Awake()
    {
        Instance = this;
    }
    

    public void PlayCard(int position, Enemy enemy, Player player, float multiplier = 1f)
    {
        Card cardToPlay = GetCard(position);
        if (cardToPlay == null)
        {
            Debug.LogWarning("No card assigned for slot " + position);
            return;
        }

        cardToPlay.PlayCard(enemy, player, position, multiplier);
        switch (position)
        {
            case 1: pos1CardUI?.PlayFlashAnimation(); break;
            case 2: pos2CardUI?.PlayFlashAnimation(); break;
            case 3: pos3CardUI?.PlayFlashAnimation(); break;
            case 4: pos4CardUI?.PlayFlashAnimation(); break;
            case 5: pos5CardUI?.PlayFlashAnimation(); break;
            case 6: pos6CardUI?.PlayFlashAnimation(); break;
        }
    }

    public void SetCard(int position, Card newCard)
    {
        Card runtimeCard = NormalizeCardReference(newCard);
        SetSlotCard(position, runtimeCard);
    }

    public Card GetCard(int index)
    {
        Card slotCard = GetSlotCard(index);
        Card runtimeCard = NormalizeCardReference(slotCard);
        if (runtimeCard != slotCard)
        {
            SetSlotCard(index, runtimeCard);
        }

        return runtimeCard;
    }

    public CardUI GetCardUI(int index)
    {
        switch (index)
        {
            case 1: return pos1CardUI;
            case 2: return pos2CardUI;
            case 3: return pos3CardUI;
            case 4: return pos4CardUI;
            case 5: return pos5CardUI;
            case 6: return pos6CardUI;
            default: Debug.LogError("Invalid card UI index: " + index); return null;
        }
    }

    private Card GetSlotCard(int index)
    {
        switch (index)
        {
            case 1: return pos1Card;
            case 2: return pos2Card;
            case 3: return pos3Card;
            case 4: return pos4Card;
            case 5: return pos5Card;
            case 6: return pos6Card;
            default: Debug.LogError("Invalid card index: " + index); return null;
        }
    }

    private void SetSlotCard(int position, Card card)
    {
        switch (position)
        {
            case 1: pos1Card = card; break;
            case 2: pos2Card = card; break;
            case 3: pos3Card = card; break;
            case 4: pos4Card = card; break;
            case 5: pos5Card = card; break;
            case 6: pos6Card = card; break;
            default: Debug.LogError("Invalid card position: " + position); break;
        }
        card.PlaceCard();
    }

    private Card NormalizeCardReference(Card card)
    {
        if (card == null) return null;
        return CardManager.Instance != null ? CardManager.Instance.CreateRuntimeCard(card) : card;
    }

    public void AddCardToRunCards(Card card)
    {
        if (card == null) return;

        Card runtimeCard = NormalizeCardReference(card);

        int newSize = runCards.Length + 1;
        Card[] newRunCards = new Card[newSize];
        for (int i = 0; i < runCards.Length; i++)
        {
            newRunCards[i] = runCards[i];
        }
        newRunCards[newSize - 1] = runtimeCard;
        runCards = newRunCards;
    }
    public Card[] GetRunCards()
    {
        if (runCards == null)
        {
            runCards = new Card[0];
        }

        for (int i = 0; i < runCards.Length; i++)
        {
            runCards[i] = NormalizeCardReference(runCards[i]);
        }

        if (runCards.Length == 0)
        {
            if (CardManager.Instance == null || CardManager.Instance.defaultCards == null)
            {
                runCards = new Card[0];
                return runCards;
            }

            Card[] runtimeDefaultCards = new Card[CardManager.Instance.defaultCards.Length];
            for (int i = 0; i < CardManager.Instance.defaultCards.Length; i++)
            {
                runtimeDefaultCards[i] = NormalizeCardReference(CardManager.Instance.defaultCards[i]);
            }
            runCards = runtimeDefaultCards;
        }
        return runCards;
    }
    void Start()
    {
        if (runCards == null)
        {
            runCards = new Card[0];
        }

        NormalizeSlotCard(1, pos1Card);
        NormalizeSlotCard(2, pos2Card);
        NormalizeSlotCard(3, pos3Card);
        NormalizeSlotCard(4, pos4Card);
        NormalizeSlotCard(5, pos5Card);
        NormalizeSlotCard(6, pos6Card);

        if (runCards.Length == 0 && CardManager.Instance != null && CardManager.Instance.defaultCards != null)
        {
            GetRunCards();
        }

        StartUp?.Invoke();
    }

    private void NormalizeSlotCard(int position, Card card)
    {
        Card runtimeCard = NormalizeCardReference(card);
        if (runtimeCard == null) return;

        Debug.Log($"Normalizing slot {position} with card: {runtimeCard.name}");
        SetSlotCard(position, runtimeCard);
    }
    
    void Update() { }
}

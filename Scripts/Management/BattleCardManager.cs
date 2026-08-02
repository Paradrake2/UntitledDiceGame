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
    
    void Awake()
    {
        Instance = this;
    }
    

    public void PlayCard(int position, Enemy enemy, Player player, float multiplier = 1f)
    {
        switch (position)
        {
            case 1:
                pos1Card.PlayCard(enemy, player, 1, multiplier);
                pos1CardUI?.PlayFlashAnimation();
                break;
            case 2:
                pos2Card.PlayCard(enemy, player, 2, multiplier);
                pos2CardUI?.PlayFlashAnimation();
                break;
            case 3:
                pos3Card.PlayCard(enemy, player, 3, multiplier);
                pos3CardUI?.PlayFlashAnimation();
                break;
            case 4:
                pos4Card.PlayCard(enemy, player, 4, multiplier);
                pos4CardUI?.PlayFlashAnimation();
                break;
            case 5:
                pos5Card.PlayCard(enemy, player, 5, multiplier);
                pos5CardUI?.PlayFlashAnimation();
                break;
            case 6:
                pos6Card.PlayCard(enemy, player, 6, multiplier);
                pos6CardUI?.PlayFlashAnimation();
                break;
            default:
                Debug.LogError("Invalid card position: " + position);
                break;
        }
    }

    public void SetCard(int position, Card newCard)
    {
        Card runtimeCard = newCard == null ? null : CardManager.Instance?.CreateRuntimeCard(newCard);
        switch (position)
        {
            case 1: pos1Card = runtimeCard; break;
            case 2: pos2Card = runtimeCard; break;
            case 3: pos3Card = runtimeCard; break;
            case 4: pos4Card = runtimeCard; break;
            case 5: pos5Card = runtimeCard; break;
            case 6: pos6Card = runtimeCard; break;
            default: Debug.LogError("Invalid card position: " + position); break;
        }
    }

    public Card GetCard(int index)
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
    public void AddCardToRunCards(Card card)
    {
        if (card == null) return;

        Card runtimeCard = card;
        if (CardManager.Instance != null && card.name != null)
        {
            runtimeCard = CardManager.Instance.CreateRuntimeCard(card);
        }

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
        if (runCards == null || runCards.Length == 0)
        {
            if (CardManager.Instance == null || CardManager.Instance.defaultCards == null)
            {
                runCards = new Card[0];
                return runCards;
            }

            Card[] runtimeDefaultCards = new Card[CardManager.Instance.defaultCards.Length];
            for (int i = 0; i < CardManager.Instance.defaultCards.Length; i++)
            {
                runtimeDefaultCards[i] = CardManager.Instance.CreateRuntimeCard(CardManager.Instance.defaultCards[i]);
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
    }

    private void NormalizeSlotCard(int position, Card card)
    {
        if (card == null) return;
        Card runtimeCard = CardManager.Instance != null ? CardManager.Instance.CreateRuntimeCard(card) : card;
        switch (position)
        {
            case 1: pos1Card = runtimeCard; break;
            case 2: pos2Card = runtimeCard; break;
            case 3: pos3Card = runtimeCard; break;
            case 4: pos4Card = runtimeCard; break;
            case 5: pos5Card = runtimeCard; break;
            case 6: pos6Card = runtimeCard; break;
        }
    }
    
    void Update() { }
}

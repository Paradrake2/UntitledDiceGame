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

    public void PlayCard(int position, Enemy enemy, Player player, float multiplier = 1f)
    {
        switch (position)
        {
            case 1:
                pos1Card.PlayCard(enemy, player, multiplier);
                pos1CardUI?.PlayFlashAnimation();
                break;
            case 2:
                pos2Card.PlayCard(enemy, player, multiplier);
                pos2CardUI?.PlayFlashAnimation();
                break;
            case 3:
                pos3Card.PlayCard(enemy, player, multiplier);
                pos3CardUI?.PlayFlashAnimation();
                break;
            case 4:
                pos4Card.PlayCard(enemy, player, multiplier);
                pos4CardUI?.PlayFlashAnimation();
                break;
            case 5:
                pos5Card.PlayCard(enemy, player, multiplier);
                pos5CardUI?.PlayFlashAnimation();
                break;
            case 6:
                pos6Card.PlayCard(enemy, player, multiplier);
                pos6CardUI?.PlayFlashAnimation();
                break;
            default:
                Debug.LogError("Invalid card position: " + position);
                break;
        }
    }

    public void SetCard(int position, Card newCard)
    {
        switch (position)
        {
            case 1: pos1Card = newCard; break;
            case 2: pos2Card = newCard; break;
            case 3: pos3Card = newCard; break;
            case 4: pos4Card = newCard; break;
            case 5: pos5Card = newCard; break;
            case 6: pos6Card = newCard; break;
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

    void Start() { }
    void Update() { }
}

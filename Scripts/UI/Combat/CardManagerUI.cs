using UnityEngine;

public class CardManagerUI : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private GameObject card1;
    [SerializeField] private GameObject card2;
    [SerializeField] private GameObject card3;
    [SerializeField] private GameObject card4;
    [SerializeField] private GameObject card5;
    [SerializeField] private GameObject card6;
    public void UpdateCardUI(int position, Card newCard)
    {
        GameObject cardObject = null;
        switch (position)
        {
            case 1:
                cardObject = card1;
                break;
            case 2:
                cardObject = card2;
                break;
            case 3:
                cardObject = card3;
                break;
            case 4:
                cardObject = card4;
                break;
            case 5:
                cardObject = card5;
                break;
            case 6:
                cardObject = card6;
                break;
            default:
                Debug.LogError("Invalid card position: " + position);
                return;
        }

        if (cardObject != null)
        {
            CardUI cardUI = cardObject.GetComponent<CardUI>();
            if (cardUI != null)
            {
                cardUI.SetCard(newCard);
            }
            else
            {
                Debug.LogError("CardUI component not found on the card GameObject.");
            }
        }
    }
    public void RefreshUI()
    {
        UpdateCardUI(1, cardManager.GetCard(1));
        UpdateCardUI(2, cardManager.GetCard(2));
        UpdateCardUI(3, cardManager.GetCard(3));
        UpdateCardUI(4, cardManager.GetCard(4));
        UpdateCardUI(5, cardManager.GetCard(5));
        UpdateCardUI(6, cardManager.GetCard(6));
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefreshUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

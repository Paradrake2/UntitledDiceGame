using UnityEngine;

public class ShopCardEquipUI : MonoBehaviour
{
    [SerializeField] private BattleCardManager bcm;
    [SerializeField] private CombatManager cm;
    [SerializeField] private Card[] selectedCards;
    [SerializeField] private CardUI[] cardUIs;
    
    void Awake()
    {
        // Initialize arrays early to prevent index out of range errors
        if (selectedCards == null || selectedCards.Length != 6)
            selectedCards = new Card[6];
            
        if (cardUIs == null || cardUIs.Length != 6)
            cardUIs = new CardUI[6];
    }
    
    void OnEnable()
    {
        ShopManager.OnShopOpened += HandleCardSelection;
        ShopManager.OnShopClosed += FinalizeCardSelection;
    }
    void OnDisable()
    {
        ShopManager.OnShopOpened -= HandleCardSelection;
        ShopManager.OnShopClosed -= FinalizeCardSelection;
    }
    // the way this is intended to work is that it populates the selectedCards array with the cards currently in the BattleCardManager when the shop is opened,
    // and then when the shop is closed, it sets all the cards in the BattleCardManager to the selected cards in the shop
    public void HandleCardSelection(bool isStart)
    {
        if (isStart)
        {
            // populate the selectedCards array with the cards currently in the BattleCardManager
            PopulateSelectedCards();
        }
    }
    private void FinalizeCardSelection()
    {
        // set all the cards in the BattleCardManager to the selected cards in the shop
        if (bcm == null)
        {
            Debug.LogError("BattleCardManager is not assigned to ShopCardEquipUI");
            return;
        }
        
        Debug.Log("Finalizing card selection. Setting BattleCardManager cards to selected cards.");
        for (int i = 0; i < 6 && i < selectedCards.Length; i++)
        {
            bcm.SetCard(i + 1, selectedCards[i]);
        }
    }

    // this is called when shop is opened
    private void PopulateSelectedCards()
    {
        // Safety checks
        if (bcm == null)
        {
            Debug.LogError("BattleCardManager is not assigned to ShopCardEquipUI");
            return;
        }
        
        if (selectedCards == null || selectedCards.Length != 6)
        {
            Debug.LogError("selectedCards array is not properly initialized");
            return;
        }
        
        if (cardUIs == null || cardUIs.Length != 6)
        {
            Debug.LogWarning("cardUIs array is not properly initialized, skipping UI updates");
        }
        
        // populate the selectedCards array with the cards currently in the BattleCardManager
        for (int i = 0; i < 6; i++)
        {
            selectedCards[i] = bcm.GetCard(i + 1);
            
            // Only update UI if cardUIs is properly initialized
            if (cardUIs != null && i < cardUIs.Length && cardUIs[i] != null)
            {
                cardUIs[i].SetCard(selectedCards[i]);
            }
        }
    }
    public void SetSelectedCard(int position, Card card)
    {
        if (position < 1 || position > 6)
        {
            Debug.LogError($"Invalid card position: {position}. Must be between 1 and 6.");
            return;
        }
        
        selectedCards[position - 1] = card;
        
        if (cardUIs != null && position - 1 < cardUIs.Length && cardUIs[position - 1] != null)
        {
            cardUIs[position - 1].SetCard(card);
        }
    }
}

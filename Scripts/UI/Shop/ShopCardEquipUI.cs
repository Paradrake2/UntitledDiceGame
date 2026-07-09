using UnityEngine;

public class ShopCardEquipUI : MonoBehaviour
{
    [SerializeField] private BattleCardManager bcm;
    [SerializeField] private CombatManager cm;
    [SerializeField] private Card[] selectedCards;
    [SerializeField] private CardUI[] cardUIs;
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
        Debug.Log("Finalizing card selection. Setting BattleCardManager cards to selected cards.");
        bcm.SetCard(1, selectedCards[0]);
        bcm.SetCard(2, selectedCards[1]);
        bcm.SetCard(3, selectedCards[2]);
        bcm.SetCard(4, selectedCards[3]);
        bcm.SetCard(5, selectedCards[4]);
        bcm.SetCard(6, selectedCards[5]);
    }

    

    // this is called when shop is opened
    private void PopulateSelectedCards()
    {
        // populate the selectedCards array with the cards currently in the BattleCardManager
        for (int i = 0; i < 6; i++)
        {
            selectedCards[i] = bcm.GetCard(i + 1);
            cardUIs[i].SetCard(selectedCards[i]);
        }
    }
    public void SetSelectedCard(int position, Card card)
    {
        selectedCards[position - 1] = card;
        cardUIs[position - 1].SetCard(card);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (selectedCards == null || selectedCards.Length != 6)
            selectedCards = new Card[6];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

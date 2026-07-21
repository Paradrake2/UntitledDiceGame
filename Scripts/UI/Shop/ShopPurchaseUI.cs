using UnityEngine;

public class ShopPurchaseUI : MonoBehaviour
{
    [SerializeField] private GameObject purchaseItemPrefab;
    [SerializeField] private Transform purchaseItemContainer;
    [SerializeField] private int numberOfPurchaseItemsToDisplay = 5; // Number of purchase items to display in the shop

    public void PopulatePurchaseItems()
    {
        foreach (Transform child in purchaseItemContainer)
            Destroy(child.gameObject);
        Card[] cardsToDisplay = ShopManager.Instance.GetPurchaseItems(numberOfPurchaseItemsToDisplay);
        // Implement the logic to populate purchase items in the shop UI
        foreach (Card card in cardsToDisplay)
        {
            GameObject purchaseItem = Instantiate(purchaseItemPrefab, purchaseItemContainer);
            // Assuming the purchaseItemPrefab has a script to set up the card details
            ShopBuyCardUI buyCardUI = purchaseItem.GetComponent<ShopBuyCardUI>();
            buyCardUI.Initialize(card);
            ShopBuyCard shopBuyCard = purchaseItem.GetComponent<ShopBuyCard>();
            shopBuyCard.SetCard(card);
        }
    }

    public void SetNumberOfPurchaseItemsToDisplay(int number)
    {
        numberOfPurchaseItemsToDisplay = number;
    }
}

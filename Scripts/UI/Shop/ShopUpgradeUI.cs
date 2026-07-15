using UnityEngine;

public class ShopUpgradeUI : MonoBehaviour
{
    [SerializeField] private GameObject upgradeItemPrefab;
    [SerializeField] private Transform upgradeItemContainer;
    [SerializeField] private int numberOfUpgradeItemsToDisplay = 5; // Number of upgrade items to display in the shop
    public void PopulateUpgradeItems()
    {
        foreach (Transform child in upgradeItemContainer)
            Destroy(child.gameObject);
        Card[] cardsToDisplay = ShopManager.Instance.GetUpgradeItems(numberOfUpgradeItemsToDisplay);
        // Implement the logic to populate upgrade items in the shop UI
        foreach (Card card in cardsToDisplay)
        {
            GameObject upgradeItem = Instantiate(upgradeItemPrefab, upgradeItemContainer);
            // Assuming the upgradeItemPrefab has a script to set up the card details
            ShopUpgradeCard upgradeCard = upgradeItem.GetComponent<ShopUpgradeCard>();
            upgradeCard.SetCard(card);
            ShopUpgradeCardUI shopUpgradeCardUI = upgradeItem.GetComponent<ShopUpgradeCardUI>();
            shopUpgradeCardUI.Instantiate(card);
        }
    }
    public void SetNumberOfUpgradeItemsToDisplay(int number)
    {
        numberOfUpgradeItemsToDisplay = number;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

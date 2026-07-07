using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Transform buyItemContainer;
    [SerializeField] private Transform upgradeItemContainer;
    [SerializeField] private Transform cardItemContainer;
    [SerializeField] private GameObject buyItemPrefab;
    [SerializeField] private GameObject upgradeItemPrefab;
    [SerializeField] private GameObject cardItemPrefab;
    [SerializeField] private CardManager cardManager;
    [SerializeField] private Transform shopPanel;
    /// <summary>The root Canvas used to reparent cards during drag so they float above all UI.</summary>
    [SerializeField] private Canvas rootCanvas;
    void OnEnable()
    {
        ShopManager.OnShopOpened += HandleShopOpened;
    }

    void OnDisable()
    {
        ShopManager.OnShopOpened -= HandleShopOpened;
    }

    private void HandleShopOpened(bool isOpened)
    {
        if (shopPanel != null)
            shopPanel.gameObject.SetActive(isOpened);

        if (isOpened)
        {
            Debug.Log("Shop opened, populating items...");
            PopulateShopItems();
            PopulateUpgradeItems();
            PopulateCardItems();
        }
    }
    // randomly chooses x number of cards from the player's unlocked card pool and displays them in the shop.
    // player can then buy the cards if they have enough coins.
    private void PopulateShopItems()
    {
        
    }
    // randomly chooses x number of cards from the player's currently owned cards. This includes the cards unlocked by default as well as the cards
    // that have been purchased this run.
    private void PopulateUpgradeItems()
    {
        
    }
    // This populates the cards that the player owns. They can equip from here by dragging the card to the equipped card slot.
    private void PopulateCardItems()
    {
        foreach (Transform child in cardItemContainer)
            Destroy(child.gameObject);

        if (cardManager == null || cardItemPrefab == null) return;

        foreach (Card card in cardManager.unlockedCards)
        {
            if (card == null) continue;
            GameObject go = Instantiate(cardItemPrefab, cardItemContainer);
            ShopDraggableCardUI draggable = go.GetComponent<ShopDraggableCardUI>();
            if (draggable != null)
                draggable.Initialize(card, rootCanvas);
        }
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

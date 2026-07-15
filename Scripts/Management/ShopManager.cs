using System;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static Action<Card> OnCardPurchased;
    public static Action<Card> OnCardSold;
    public static Action<Card> OnCardUpgraded;
    public static Action<bool> OnShopOpened;
    public static Action OnShopClosed;
    [SerializeField] private ShopUI shopUI;
    [SerializeField] private Player player;
    private static ShopManager instance;
    public static ShopManager Instance => instance;
    void OnEnable()
    {
        OnShopOpened += RunShop;
        OnCardUpgraded += UpgradeCard;
    }
    void OnDisable()
    {
        OnShopOpened -= RunShop;
        OnCardUpgraded -= UpgradeCard;
    }
    public void OpenShop()
    {
        OnShopOpened?.Invoke(true);
    }
    public void CloseShop()
    {
        OnShopOpened?.Invoke(false);
        OnShopClosed?.Invoke();
        Debug.Log("Shop closed. Starting next battle...");
        // Start the next battle after all shop-close subscribers (e.g. FinalizeCardSelection) have run.
    }
    public void RunShop(bool isOpened)
    {
        //shopUI.gameObject.SetActive(isOpened);
        if (!isOpened)
        {
            // Shop is closing, finalize card selection
            OnShopClosed?.Invoke();
            CombatManager.Instance?.StartNextBattle();

        }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void UpgradeCard(Card card)
    {
        if (card == null)
        {
            Debug.LogError("Cannot upgrade a null card.");
            return;
        }

        int upgradeCost = card.GetUpgradeCost();
        if (player.Coins >= upgradeCost)
        {
            player.SpendCoins(upgradeCost);
            card.UpgradeCard();
            Debug.Log($"Upgraded card: {card.name}. New level: {card.UpgradeLevel}. Coins left: {player.Coins}");
        }
        else
        {
            Debug.LogWarning($"Not enough coins to upgrade {card.name}. Required: {upgradeCost}, Available: {player.Coins}");
        }
    }
    public Card[] GetUpgradeItems(int numberOfItems)
    {
        Debug.Log("GetUpgradeItems called");
        
        // Safety check: ensure BattleCardManager is initialized
        if (BattleCardManager.Instance == null)
        {
            Debug.LogError("BattleCardManager.Instance is not initialized. Returning empty array.");
            return new Card[0];
        }
        
        Card[] availableCards = BattleCardManager.Instance.GetRunCards(); // Get the cards the player has this run
        
        // If there are no cards available, return empty array
        if (availableCards == null || availableCards.Length == 0)
        {
            Debug.LogWarning("No available cards for upgrades. Returning empty array.");
            return new Card[0];
        }
        
        // Limit numberOfItems to the number of available cards to avoid infinite loop
        int itemsToReturn = Mathf.Min(numberOfItems, availableCards.Length);
        Card[] returnedCards = new Card[itemsToReturn];
        
        for (int i = 0; i < itemsToReturn; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableCards.Length);
            Card randomCard = availableCards[randomIndex];
            int attempts = 0;
            
            // if random card has already been selected, choose another one (with attempt limit to avoid infinite loop)
            while (System.Array.Exists(returnedCards, card => card == randomCard) && attempts < 100)
            {
                randomIndex = UnityEngine.Random.Range(0, availableCards.Length);
                randomCard = availableCards[randomIndex];
                attempts++;
            }
            returnedCards[i] = randomCard;
        }
        return returnedCards;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnShopOpened?.Invoke(true); // open shop at start of game so player can select starting cards
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

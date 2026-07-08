using System;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static Action<Card> OnCardPurchased;
    public static Action<Card> OnCardSold;
    public static Action<Card> OnCardUpgraded;
    public static Action<bool> OnShopOpened;
    [SerializeField] private ShopUI shopUI;
    [SerializeField] private Player player;
    private static ShopManager instance;
    public static ShopManager Instance => instance;
    void OnEnable()
    {
        OnCardPurchased += HandleCardPurchased;
        OnShopOpened += RunShop;
    }
    void OnDisable()
    {
        OnCardPurchased -= HandleCardPurchased;
        OnShopOpened -= RunShop;
    }
    public void OpenShop()
    {
        OnShopOpened?.Invoke(true);
    }
    public void CloseShop()
    {
        OnShopOpened?.Invoke(false);
        Debug.Log("Shop closed. Starting next battle...");
        // Start the next battle after all shop-close subscribers (e.g. FinalizeCardSelection) have run.
    }
    public void RunShop(bool isOpened)
    {
        //shopUI.gameObject.SetActive(isOpened);
        if (!isOpened)
        {
            // Shop is closing, finalize card selection
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
    private void HandleCardPurchased(Card card)
    {
        // put it in the player's deck
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

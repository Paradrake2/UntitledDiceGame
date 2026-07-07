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
        OnShopOpened += OpenShopTest;
    }
    void OnDisable()
    {
        OnCardPurchased -= HandleCardPurchased;
        OnShopOpened -= OpenShopTest;
    }
    public void OpenShop()
    {
        OnShopOpened?.Invoke(true);
    }
    public void CloseShop()
    {
        OnShopOpened?.Invoke(false);
        // Start the next battle after all shop-close subscribers (e.g. FinalizeCardSelection) have run.
        CombatManager.Instance?.StartNextBattle();
    }
    public void OpenShopTest(bool isOpened)
    {
        shopUI.gameObject.SetActive(true);
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

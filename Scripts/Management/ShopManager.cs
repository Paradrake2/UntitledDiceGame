using System;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static Action<Card> OnCardPurchased;
    public static Action<Card> OnCardSold;
    public static Action<Card> OnCardUpgraded;
    public static Action<bool> OnShopOpened;
    private static ShopManager instance;
    public static ShopManager Instance => instance;
    public void OpenShop()
    {
        OnShopOpened?.Invoke(true);
    }
    public void CloseShop()
    {
        OnShopOpened?.Invoke(false);
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

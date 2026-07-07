using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Transform buyItemContainer;
    [SerializeField] private Transform upgradeItemContainer;
    [SerializeField] private GameObject buyItemPrefab;
    [SerializeField] private GameObject upgradeItemPrefab;
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
        gameObject.SetActive(isOpened);
        if (isOpened)
        {
            PopulateShopItems();
            PopulateUpgradeItems();
        }
    }
    private void PopulateShopItems()
    {
        
    }
    private void PopulateUpgradeItems()
    {
        
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

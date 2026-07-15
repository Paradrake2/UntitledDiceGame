using UnityEngine;

public class ShopUpgradeManager
{
    private static ShopUpgradeManager instance;
    public static ShopUpgradeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ShopUpgradeManager();
            }
            return instance;
        }
    }
    
}

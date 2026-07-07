using UnityEngine;

public class ConfirmButton : MonoBehaviour
{
    public void OnClick()
    {
        // Call the method to confirm the loadout selection
        ShopManager.OnShopOpened?.Invoke(false); // Close the shop
    }
}

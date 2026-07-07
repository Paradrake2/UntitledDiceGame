using UnityEngine;
using UnityEngine.UI;

public class ShopBuyCard : MonoBehaviour
{
    [SerializeField] private Card card;
    public void OnPurchaseClicked()
    {
        ShopManager.OnCardPurchased?.Invoke(card);
    }
}

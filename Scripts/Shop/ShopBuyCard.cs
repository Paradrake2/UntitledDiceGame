using UnityEngine;
using UnityEngine.UI;

public class ShopBuyCard : MonoBehaviour
{
    [SerializeField] private Card card;
    public void OnPurchaseClicked()
    {
        ShopManager.OnCardPurchased?.Invoke(card);
        this.gameObject.SetActive(false); // Hide the purchase item after purchase
    }
    public void SetCard(Card card)
    {
        this.card = card;
    }
}

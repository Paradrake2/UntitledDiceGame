using UnityEngine;

public class ShopUpgradeCard : MonoBehaviour
{
    [SerializeField] private Card card; // The card associated with this upgrade item.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetCard(Card newCard)
    {
        card = newCard;
    }
    public void UpgradeCard()
    {
        ShopManager.OnCardUpgraded?.Invoke(card);
    }
}

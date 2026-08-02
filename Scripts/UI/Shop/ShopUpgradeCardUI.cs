using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUpgradeCardUI : MonoBehaviour
{
    [SerializeField] private ShopUpgradeCard shopUpgradeCard;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image icon;
    public void OnClick()
    {
        // upgrade card
        shopUpgradeCard.UpgradeCard();
    }
    public void Instantiate(Card card)
    {
        shopUpgradeCard.SetCard(card);
        priceText.text = card.GetUpgradeCost().ToString();
        icon.sprite = card.CardSprite;
    }
    public void Refresh()
    {
        Card card = shopUpgradeCard.GetCard();
        if (card != null)
        {
            priceText.text = card.GetUpgradeCost().ToString();
            icon.sprite = card.CardSprite;
        }
    }
    public Card GetCard()
    {
        return shopUpgradeCard.GetCard();
    }
}

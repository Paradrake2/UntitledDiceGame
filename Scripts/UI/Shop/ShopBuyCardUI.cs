using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopBuyCardUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Card card;
    [SerializeField] private TextMeshProUGUI costText;
    public void Initialize(Card card)
    {
        this.card = card;

        if (icon != null && card != null && card.CardSprite != null)
            icon.sprite = card.CardSprite;
        costText.text = card.BaseShopCost.ToString();
    }
}

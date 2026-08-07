using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopUpgradeCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
            ShopDescription.Instance.UpdateDescription(card);
        }
    }
    public Card GetCard()
    {
        return shopUpgradeCard.GetCard();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShopDescription.Instance.UpdateDescription(GetCard());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShopDescription.Instance.UpdateDescription(null);
    }
}

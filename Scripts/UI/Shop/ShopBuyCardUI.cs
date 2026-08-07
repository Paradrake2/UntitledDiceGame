using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class ShopBuyCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShopDescription.Instance.UpdateDescription(card);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShopDescription.Instance.UpdateDescription(null);
    }
}

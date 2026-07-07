using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Attach to each card prefab in the unlocked-cards list.
/// Drag it onto a ShopLoadoutSlotUI to equip it for the next battle.
/// Requires a CanvasGroup component on the same GameObject.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class ShopDraggableCardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image cardImage;

    private Card card;
    private Canvas rootCanvas;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector2 originalAnchoredPosition;
    private bool dropHandled;

    public Card Card => card;

    public void Initialize(Card card, Canvas rootCanvas)
    {
        this.card = card;
        this.rootCanvas = rootCanvas;
        canvasGroup = GetComponent<CanvasGroup>();

        if (cardImage != null && card?.CardSprite != null)
            cardImage.sprite = card.CardSprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dropHandled = false;
        originalParent = transform.parent;
        originalAnchoredPosition = ((RectTransform)transform).anchoredPosition;

        // Reparent to root canvas so the card floats above all other UI.
        transform.SetParent(rootCanvas.transform, true);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 0.75f;
        canvasGroup.blocksRaycasts = false; // lets pointer events reach the drop zone beneath
    }

    public void OnDrag(PointerEventData eventData)
    {
        // delta-based movement works correctly regardless of canvas render mode or scale
        ((RectTransform)transform).anchoredPosition += eventData.delta / rootCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (!dropHandled)
            ReturnToOrigin();
    }

    /// <summary>Called by ShopLoadoutSlotUI when a successful drop occurs.</summary>
    public void NotifyDropHandled() => dropHandled = true;

    public void ReturnToOrigin()
    {
        transform.SetParent(originalParent, false);
        ((RectTransform)transform).anchoredPosition = originalAnchoredPosition;
    }
}

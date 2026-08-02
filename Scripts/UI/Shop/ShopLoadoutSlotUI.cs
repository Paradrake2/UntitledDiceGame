using UnityEngine;
using UnityEngine.EventSystems;

public class ShopLoadoutSlotUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private int slotIndex; // 1–6
    [SerializeField] private ShopCardEquipUI equipUI;

    public void OnDrop(PointerEventData eventData)
    {
        ShopDraggableCardUI dragged = eventData.pointerDrag?.GetComponent<ShopDraggableCardUI>();
        if (dragged == null || dragged.Card == null) return;

        equipUI.SetSelectedCard(slotIndex, dragged.Card);
        dragged.NotifyDropHandled();
        dragged.ReturnToOrigin();
    }
}

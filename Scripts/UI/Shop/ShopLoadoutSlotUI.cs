using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Attach to each of the 6 loadout slot GameObjects on the right-side panel.
/// Set slotIndex (1–6) and equipUI in the Inspector.
/// When a ShopDraggableCardUI is dropped here, it updates the battle loadout.
/// </summary>
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

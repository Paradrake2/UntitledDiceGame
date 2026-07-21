using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UpgradeSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button purchaseButton;

    [SerializeField] private Upgrade upgrade;
    [SerializeField] private UpgradeMenuUI menu;

    public void Initialize(Upgrade upgrade, UpgradeMenuUI menu)
    {
        this.upgrade = upgrade;
        this.menu = menu;

        if (icon != null && upgrade.Icon != null)
            icon.sprite = upgrade.Icon;

        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(OnPurchaseClicked);

        Refresh();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Show upgrade description in the menu's description panel
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // clear description
        throw new System.NotImplementedException();
    }

    // Updates level, cost, and button interactability to reflect the current state.
    public void Refresh()
    {
        if (upgrade == null || UpgradeManager.Instance == null) return;

        int level = UpgradeManager.Instance.GetLevel(upgrade);
        bool maxed = level >= upgrade.MaxLevel;

        levelText.text = maxed ? "MAX" : $"{level}/{upgrade.MaxLevel}";
        costText.text = maxed ? "" : $"{upgrade.GemCostForNextLevel(level)} gems";
        purchaseButton.interactable = !maxed && UpgradeManager.Instance.CanPurchase(upgrade);
    }

    private void OnPurchaseClicked()
    {
        if (UpgradeManager.Instance.TryPurchase(upgrade))
            menu.RefreshAll();
    }
}

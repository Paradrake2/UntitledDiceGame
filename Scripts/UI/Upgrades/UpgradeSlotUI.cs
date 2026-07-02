using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Represents one upgrade entry in the upgrade menu.
/// Assign the child UI elements in the Inspector, then call Initialize() from UpgradeMenuUI.
/// </summary>
public class UpgradeSlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button purchaseButton;

    private Upgrade upgrade;
    private UpgradeMenuUI menu;

    public void Initialize(Upgrade upgrade, UpgradeMenuUI menu)
    {
        this.upgrade = upgrade;
        this.menu = menu;

        if (icon != null && upgrade.Icon != null)
            icon.sprite = upgrade.Icon;

        nameText.text = upgrade.UpgradeName;
        descriptionText.text = upgrade.Description;

        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(OnPurchaseClicked);

        Refresh();
    }

    /// <summary>Updates level, cost, and button interactability to reflect the current state.</summary>
    public void Refresh()
    {
        if (upgrade == null || UpgradeManager.Instance == null) return;

        int level = UpgradeManager.Instance.GetLevel(upgrade);
        bool maxed = level >= upgrade.MaxLevel;

        levelText.text = maxed ? "MAX" : $"Level {level} / {upgrade.MaxLevel}";
        costText.text = maxed ? "" : $"{upgrade.GemCostForNextLevel(level)} gems";
        purchaseButton.interactable = !maxed && UpgradeManager.Instance.CanPurchase(upgrade);
    }

    private void OnPurchaseClicked()
    {
        if (UpgradeManager.Instance.TryPurchase(upgrade))
            menu.RefreshAll();
    }
}

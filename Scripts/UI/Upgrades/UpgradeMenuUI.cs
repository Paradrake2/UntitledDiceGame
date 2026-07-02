using UnityEngine;
using TMPro;

/// <summary>
/// Controller for the upgrade menu screen.
/// Spawns one UpgradeSlotUI per available upgrade and keeps the gem display up to date.
/// Wire up in the Inspector: assign upgradeSlotPrefab, slotContainer, gemText, and this GameObject.
/// Call Open() / Close() from your menu navigation buttons.
/// </summary>
public class UpgradeMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject upgradeSlotPrefab;
    [SerializeField] private Transform slotContainer;
    [SerializeField] private TextMeshProUGUI gemText;

    private UpgradeSlotUI[] slots;

    private void OnEnable()
    {
        BuildSlots();
        RefreshAll();
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    /// <summary>Refreshes gem display and every upgrade slot.</summary>
    public void RefreshAll()
    {
        if (UpgradeManager.Instance != null && gemText != null)
            gemText.text = $"Gems: {UpgradeManager.Instance.Gems}";

        if (slots == null) return;
        foreach (UpgradeSlotUI slot in slots)
            slot.Refresh();
    }

    private void BuildSlots()
    {
        if (UpgradeManager.Instance == null || upgradeSlotPrefab == null || slotContainer == null)
            return;

        // Clear existing children
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);

        Upgrade[] upgrades = UpgradeManager.Instance.AvailableUpgrades;
        slots = new UpgradeSlotUI[upgrades.Length];

        for (int i = 0; i < upgrades.Length; i++)
        {
            GameObject go = Instantiate(upgradeSlotPrefab, slotContainer);
            slots[i] = go.GetComponent<UpgradeSlotUI>();
            slots[i].Initialize(upgrades[i], this);
        }
    }
}

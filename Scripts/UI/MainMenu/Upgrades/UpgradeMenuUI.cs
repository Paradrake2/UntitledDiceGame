using UnityEngine;
using TMPro;

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

    // Refreshes gem display and every upgrade slot.
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

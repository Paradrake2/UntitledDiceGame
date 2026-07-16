using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Persistent singleton that owns gem currency and all purchased upgrade levels.
/// Data is saved to PlayerPrefs so it survives between sessions.
/// Place this on a GameObject in a persistent scene (or use DontDestroyOnLoad).
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [SerializeField] private Upgrade[] availableUpgrades;

    /// <summary>Fired whenever a gem upgrade is successfully purchased.</summary>
    public static event Action OnUpgradePurchased;

    private int gems;
    private readonly Dictionary<string, int> purchasedLevels = new Dictionary<string, int>();

    public int Gems => gems;
    public Upgrade[] AvailableUpgrades => availableUpgrades;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }

    // ── Querying ──────────────────────────────────────────────────────────────

    public int GetLevel(Upgrade upgrade)
    {
        return purchasedLevels.TryGetValue(upgrade.UpgradeName, out int level) ? level : 0;
    }

    public bool CanPurchase(Upgrade upgrade)
    {
        int level = GetLevel(upgrade);
        return level < upgrade.MaxLevel && gems >= upgrade.GemCostForNextLevel(level);
    }

    /// <summary>
    /// Sum of (valuePerLevel * purchasedLevel) across all upgrades of the given type, rounded to int.
    /// Use for whole-number stats: damage bonus, damage reduction, extra dice, extra rerolls, revives, health bonus, shield bonus, gems bonus, health regen, shield regen.
    /// </summary>
    public int GetTotalInt(UpgradeType type)
    {
        int total = 0;
        foreach (Upgrade upgrade in availableUpgrades)
        {
            if (upgrade.UpgradeType == type)
                total += Mathf.RoundToInt(upgrade.ValuePerLevel * GetLevel(upgrade));
        }
        return total;
    }
    /// <summary>
    /// Sum of (valuePerLevel * purchasedLevel) across all upgrades of the given type.
    /// Use for fractional stats: shop discount (0–1 range).
    /// </summary>
    public float GetTotalFloat(UpgradeType type)
    {
        float total = 0f;
        foreach (Upgrade upgrade in availableUpgrades)
        {
            if (upgrade.UpgradeType == type)
                total += upgrade.ValuePerLevel * GetLevel(upgrade);
        }
        return total;
    }

    // ── Purchasing ────────────────────────────────────────────────────────────

    /// <summary>Attempts to purchase the next level of an upgrade. Returns true on success.</summary>
    public bool TryPurchase(Upgrade upgrade)
    {
        if (!CanPurchase(upgrade)) return false;

        int level = GetLevel(upgrade);
        gems -= upgrade.GemCostForNextLevel(level);
        purchasedLevels[upgrade.UpgradeName] = level + 1;
        OnUpgradePurchased?.Invoke();
        Save();
        return true;
    }

    // ── Gem management ────────────────────────────────────────────────────────

    public void AddGems(int amount)
    {
        gems += amount;
        Save();
    }

    // ── Persistence ───────────────────────────────────────────────────────────

    private void Save()
    {
        PlayerPrefs.SetInt("Gems", gems);
        foreach (var kvp in purchasedLevels)
            PlayerPrefs.SetInt("Upgrade_" + kvp.Key, kvp.Value);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        gems = PlayerPrefs.GetInt("Gems", 0);
        purchasedLevels.Clear();
        if (availableUpgrades == null) return;
        foreach (Upgrade upgrade in availableUpgrades)
        {
            int level = PlayerPrefs.GetInt("Upgrade_" + upgrade.UpgradeName, 0);
            if (level > 0)
                purchasedLevels[upgrade.UpgradeName] = level;
        }
    }
}

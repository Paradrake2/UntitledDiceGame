using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrades/Upgrade")]
public class Upgrade : ScriptableObject
{
    [SerializeField] private string upgradeName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private UpgradeType upgradeType;
    [SerializeField] private int maxLevel = 10;
    /// <summary>Base gem cost for level 1. Each subsequent level costs an additional gemCostPerLevel.</summary>
    [SerializeField] private int gemCostPerLevel = 10;
    /// <summary>How much the upgrade's effect grows per purchased level.</summary>
    [SerializeField] private float valuePerLevel = 1f;

    public string UpgradeName => upgradeName;
    public string Description => description;
    public Sprite Icon => icon;
    public UpgradeType UpgradeType => upgradeType;
    public int MaxLevel => maxLevel;
    public float ValuePerLevel => valuePerLevel;

    /// <summary>Gem cost to go from currentLevel to currentLevel + 1.</summary>
    public int GemCostForNextLevel(int currentLevel)
    {
        return gemCostPerLevel * (currentLevel + 1);
    }
}

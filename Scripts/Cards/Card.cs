using UnityEngine;

[System.Serializable]
public class CardStats
{
    public int damage;
    public int shield;
    public int heal;
    public float percentage1;
    public int int1;
}


//[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public abstract class Card : ScriptableObject
{
    [SerializeField] protected string cardName;
    [SerializeField] protected string cardDescription;
    [SerializeField] protected Sprite cardSprite;

    [Header("Base Stats (at level 0)")]
    [SerializeField] protected CardStats baseStats;

    [Header("Stat Increases Per Level")]
    [SerializeField] protected CardStats statsPerLevel;

    [Header("Card Text Templates")]
    [Tooltip("Use {damage}, {shield}, {heal}, {percentage1}, {int1} as placeholders. Leave empty to hide the line.")]
    [SerializeField] protected string line1Template;
    [SerializeField] protected string line2Template;
    [SerializeField] protected string line3Template;

    [SerializeField] protected int upgradeLevel;
    [SerializeField] protected int baseUpgradeCost;
    [SerializeField] protected int baseShopCost;
    [SerializeField] protected int maxUpgradeLevel = 10;
    [SerializeField] protected Color line1Color = Color.white;
    [SerializeField] protected Color line2Color = Color.white;
    [SerializeField] protected Color line3Color = Color.white;
    [SerializeField] protected bool isUnlockedByDefault = false;

    // Computed stats for the current upgrade level
    public int Damage       => baseStats.damage      + statsPerLevel.damage      * upgradeLevel;
    public int Shield       => baseStats.shield      + statsPerLevel.shield      * upgradeLevel;
    public int Heal         => baseStats.heal        + statsPerLevel.heal        * upgradeLevel;
    public float Percentage1 => baseStats.percentage1 + statsPerLevel.percentage1 * upgradeLevel;
    public int Int1         => baseStats.int1        + statsPerLevel.int1        * upgradeLevel;

    public string CardName => cardName;
    public string CardDescription => cardDescription;
    public Sprite CardSprite => cardSprite;

    public string Line1 => FormatTemplate(line1Template);
    public string Line2 => FormatTemplate(line2Template);
    public string Line3 => FormatTemplate(line3Template);

    public Color Line1Color => line1Color;
    public Color Line2Color => line2Color;
    public Color Line3Color => line3Color;
    public int UpgradeLevel => upgradeLevel;
    public int MaxUpgradeLevel => maxUpgradeLevel;
    public bool IsUnlockedByDefault => isUnlockedByDefault;
    public int BaseUpgradeCost => baseUpgradeCost;
    public int BaseShopCost => baseShopCost;

    private string FormatTemplate(string template)
    {
        if (string.IsNullOrEmpty(template)) return string.Empty;
        return template
            .Replace("{damage}",      Damage.ToString())
            .Replace("{shield}",      Shield.ToString())
            .Replace("{heal}",        Heal.ToString())
            .Replace("{percentage1}", (Percentage1 * 100).ToString("0.##") + "%")
            .Replace("{int1}",        Int1.ToString());
    }

    public abstract void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1f);

    public virtual int GetUpgradeCost()
    {
        return baseUpgradeCost * (upgradeLevel + 1);
    }

    public void UpgradeCard()
    {
        if (upgradeLevel < maxUpgradeLevel)
        {
            upgradeLevel++;
        }
        else
        {
            Debug.LogWarning("Card is already at max upgrade level.");
        }
    }
}

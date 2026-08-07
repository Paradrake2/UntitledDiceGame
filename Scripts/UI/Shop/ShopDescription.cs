using UnityEngine;
using TMPro;

public class ShopDescription : MonoBehaviour
{
    public static ShopDescription Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI descriptionText;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void UpdateDescription(Card card)
    {
        if (card != null)
        {
            if (card.IsMaxLevel())
            {
                descriptionText.text = card.CardName
                + "\n" + "Current level: " + card.UpgradeLevel + "\n" +
                CurrentFormatTemplate(card.CardDescription, card)
                + "\n \n \n"
                + "Next level: MAX LEVEL"
                + "\n \n \n" + "Upgrade cost: N/A"
                ;
                return;
            }
            descriptionText.text = card.CardName
            + "\n" + "Current level: " + card.UpgradeLevel + "\n" +
            CurrentFormatTemplate(card.CardDescription, card)
            + "\n \n \n"
            + "Next level: " + (card.UpgradeLevel + 1) + "\n" +
            UpgradeFormatTemplate(card.CardDescription, card)
            + "\n \n \n" + "Upgrade cost: " + card.GetUpgradeCost()
            ;
        }
        else
        {
            descriptionText.text = "";
        }
    }
    private string CurrentFormatTemplate(string template, Card card)
    {
        if (string.IsNullOrEmpty(template)) return string.Empty;
        return template
            .Replace("{damage}",      FormatValue(card.Damage.ToString(), "#CA0000"))
            .Replace("{shield}",      FormatValue(card.Shield.ToString(), "#004f8f"))
            .Replace("{heal}",        FormatValue(card.Heal.ToString(), "#00A000"))
            .Replace("{percentage1}", FormatValue((card.Percentage1 * 100).ToString("0.##") + "%", "#d700df"))
            .Replace("{int1}",        FormatValue(card.Int1.ToString(), "#FFD166"));
    }
    private string UpgradeFormatTemplate(string template, Card card)
    {
        if (string.IsNullOrEmpty(template)) return string.Empty;
        return template
            .Replace("{damage}",      FormatValue(NextLevelDamage(card).ToString()))
            .Replace("{shield}",      FormatValue(NextLevelShield(card).ToString()))
            .Replace("{heal}",        FormatValue(NextLevelHeal(card).ToString()))
            .Replace("{percentage1}", FormatValue((NextLevelPercentage1(card) * 100).ToString("0.##") + "%"))
            .Replace("{int1}",        FormatValue(NextLevelInt1(card).ToString()));
    }

    private string FormatValue(string value, string color = "#FFD166")
    {
        return $"<b><{color}><size=120%>{value}</size></color></b>";
    }
    
    private int NextLevelDamage(Card card)
    {
        return card.Damage + card.StatsPerLevel.damage;
    }
    private int NextLevelShield(Card card)
    {
        return card.Shield + card.StatsPerLevel.shield;
    }
    private int NextLevelHeal(Card card)
    {
        return card.Heal + card.StatsPerLevel.heal;
    }
    private float NextLevelPercentage1(Card card)
    {
        return card.Percentage1 + card.StatsPerLevel.percentage1;
    }
    private int NextLevelInt1(Card card)
    {
        return card.Int1 + card.StatsPerLevel.int1;
    }
}

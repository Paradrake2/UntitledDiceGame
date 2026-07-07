using UnityEngine;

[System.Serializable]
public class CardUpgradeLevels
{
    public int damage;
    public int shield;
    public int heal;
    public float percentage1;
    public int int1;
    public string line1;
    public string line2;
    public string line3;
}


//[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public abstract class Card : ScriptableObject
{
    [SerializeField] protected string cardName;
    [SerializeField] protected string cardDescription;
    [SerializeField] protected Sprite cardSprite;
    [SerializeField] protected CardUpgradeLevels[] upgradeLevels;
    [SerializeField] protected int upgradeLevel;
    [SerializeField] protected int baseUpgradeCost;
    [SerializeField] protected int baseShopCost;
    [SerializeField] protected int maxUpgradeLevel = 10;
    [SerializeField] protected Color line1Color = Color.white;
    [SerializeField] protected Color line2Color = Color.white;
    [SerializeField] protected Color line3Color = Color.white;
    public string CardName => cardName;
    public string CardDescription => cardDescription;
    public Sprite CardSprite => cardSprite;
    public string Line1 => upgradeLevels[upgradeLevel].line1;
    public string Line2 => upgradeLevels[upgradeLevel].line2;
    public string Line3 => upgradeLevels[upgradeLevel].line3;
    public Color Line1Color => line1Color;
    public Color Line2Color => line2Color;
    public Color Line3Color => line3Color;
    public abstract void PlayCard(Enemy enemy, Player player, float multiplier = 1f);
}

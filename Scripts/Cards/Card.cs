using UnityEngine;

[System.Serializable]
public class CardUpgradeLevels
{
    public int damage;
    public int shield;
    public int heal;
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
    public string CardName => cardName;
    public string CardDescription => cardDescription;
    public Sprite CardSprite => cardSprite;
    public abstract void PlayCard(Enemy enemy, Player player, float multiplier = 1f);
}

using UnityEngine;

[CreateAssetMenu(fileName = "DestinyDice", menuName = "Cards/DestinyDice")]
public class DestinyDice : Card
{
    // deals damage based on a roll of the secondary die, multiplied by the card's damage value
    public override void PlayCard(Enemy enemy, Player player, int index = 1, float multiplier = 1f)
    {
        int damage = Mathf.RoundToInt(upgradeLevels[upgradeLevel].int1 * multiplier);
        int roll = DiceManager.Instance.RollSecondaryDie();
        int finalDamage = damage * roll;
        DamageManager.Instance.ApplyDamageToEnemy(enemy, player, finalDamage, false);
    }
}

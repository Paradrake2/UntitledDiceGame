using UnityEngine;

[CreateAssetMenu(fileName = "DestinyDice", menuName = "Cards/DestinyDice")]
public class DestinyDice : Card
{
    public override void PlayCard(Enemy enemy, Player player, float multiplier = 1)
    {
        int damage = Mathf.RoundToInt(upgradeLevels[upgradeLevel].int1 * multiplier);
        int roll = DiceManager.Instance.RollSecondaryDie();
        int finalDamage = damage * roll;
        var ctx = new StatusEffectContext(player, enemy, isPlayerEffect: true);
        finalDamage = player.StatusEffects.ModifyOutgoingDamage(finalDamage, false, ctx);
        enemy.TakeDamage(finalDamage, false);
    }
}

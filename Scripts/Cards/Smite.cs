using UnityEngine;

[CreateAssetMenu(fileName = "Smite", menuName = "Cards/Smite")]
public class Smite : Card
{
    // deals both physical and magic damage, secondary dice gives boost to either magic or physical depending on even or odd roll
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1f)
    {
        int damage = Mathf.RoundToInt(upgradeLevels[upgradeLevel].damage * multiplier) + player.OutgoingDamageBonus;
        var ctx = new StatusEffectContext(player, enemy, isPlayerEffect: true);
        damage = player.StatusEffects.ModifyOutgoingDamage(damage, false, ctx);
        enemy.TakeDamage(damage, false);

        int roll = DiceManager.Instance.RollSecondaryDie();
        if (roll % 2 == 0)
        {
            // even roll, deal magic damage
            int magicDamage = Mathf.RoundToInt(upgradeLevels[upgradeLevel].int1 * multiplier) + player.OutgoingDamageBonus;
            magicDamage = player.StatusEffects.ModifyOutgoingDamage(magicDamage, false, ctx);
            enemy.TakeDamage(magicDamage, true);
        }
        else
        {
            // odd roll, deal physical damage
            int physicalDamage = Mathf.RoundToInt(upgradeLevels[upgradeLevel].int1 * multiplier) + player.OutgoingDamageBonus;
            physicalDamage = player.StatusEffects.ModifyOutgoingDamage(physicalDamage, false, ctx);
            enemy.TakeDamage(physicalDamage, false);
        }
    }
}

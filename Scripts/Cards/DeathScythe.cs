using UnityEngine;

[CreateAssetMenu(fileName = "DeathScythe", menuName = "Cards/DeathScythe")]
public class DeathScythe : Card
{
    // deals damage and heals player for a percentage of the damage dealt
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1f)
    {
        int damage = Mathf.RoundToInt(upgradeLevels[upgradeLevel].damage * multiplier) + player.OutgoingDamageBonus;
        var ctx = new StatusEffectContext(player, enemy, isPlayerEffect: true);
        damage = player.StatusEffects.ModifyOutgoingDamage(damage, false, ctx);
        enemy.TakeDamage(damage, false);
        int healAmount = Mathf.RoundToInt(upgradeLevels[upgradeLevel].percentage1 * damage);
        player.Heal(healAmount);
    }
}

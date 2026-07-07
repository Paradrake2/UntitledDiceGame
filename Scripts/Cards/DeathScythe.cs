using UnityEngine;

[CreateAssetMenu(fileName = "DeathScythe", menuName = "Cards/DeathScythe")]
public class DeathScythe : Card
{
    
    public override void PlayCard(Enemy enemy, Player player, float multiplier = 1)
    {
        int damage = Mathf.RoundToInt(upgradeLevels[upgradeLevel].damage * multiplier) + player.OutgoingDamageBonus;
        var ctx = new StatusEffectContext(player, enemy, isPlayerEffect: true);
        damage = player.StatusEffects.ModifyOutgoingDamage(damage, false, ctx);
        enemy.TakeDamage(damage, false);
        int healAmount = Mathf.RoundToInt(upgradeLevels[upgradeLevel].percentage1 * damage);
        player.Heal(healAmount);
    }
}

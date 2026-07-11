using UnityEngine;

[CreateAssetMenu(fileName = "Stab", menuName = "Cards/Stab")]
public class Stab : Card
{
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1f)
    {
        int damage = Mathf.RoundToInt(upgradeLevels[upgradeLevel].damage * multiplier) + player.OutgoingDamageBonus;
        var ctx = new StatusEffectContext(player, enemy, isPlayerEffect: true);
        damage = player.StatusEffects.ModifyOutgoingDamage(damage, false, ctx);
        enemy.TakeDamage(damage, false);
    }
}

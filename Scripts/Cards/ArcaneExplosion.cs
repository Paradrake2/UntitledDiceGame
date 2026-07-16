using UnityEngine;

[CreateAssetMenu(fileName = "ArcaneExplosion", menuName = "Cards/ArcaneExplosion")]
public class ArcaneExplosion : Card
{
    // deals large amount of magic damage to enemy and small amount of damage to player
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        int damage = Mathf.RoundToInt(Damage * multiplier) + player.OutgoingDamageBonus;
        DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage, true, index);
        int selfDamage = Mathf.RoundToInt(damage * Percentage1);
        DamageManager.Instance.ApplyDamageToPlayer(player, selfDamage, true);
    }
}

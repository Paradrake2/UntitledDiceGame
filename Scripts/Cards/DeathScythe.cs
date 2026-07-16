using UnityEngine;

[CreateAssetMenu(fileName = "DeathScythe", menuName = "Cards/DeathScythe")]
public class DeathScythe : Card
{
    // deals damage and heals player for a percentage of the damage dealt
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1f)
    {
        int damage = Mathf.RoundToInt(Damage * multiplier) + player.OutgoingDamageBonus;
        DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage, false, index);
        int healAmount = Mathf.RoundToInt(Percentage1 * damage);
        player.Heal(healAmount);
    }
}

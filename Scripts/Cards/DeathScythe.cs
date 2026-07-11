using UnityEngine;

[CreateAssetMenu(fileName = "DeathScythe", menuName = "Cards/DeathScythe")]
public class DeathScythe : Card
{
    // deals damage and heals player for a percentage of the damage dealt
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1f)
    {
        int damage = Mathf.RoundToInt(upgradeLevels[upgradeLevel].damage * multiplier) + player.OutgoingDamageBonus;
        DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage, false);
        int healAmount = Mathf.RoundToInt(upgradeLevels[upgradeLevel].percentage1 * damage);
        player.Heal(healAmount);
    }
}

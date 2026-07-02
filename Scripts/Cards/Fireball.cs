using UnityEngine;

[CreateAssetMenu(fileName = "Fireball", menuName = "Cards/Fireball")]
public class Fireball : Card
{
    public override void PlayCard(Enemy enemy, Player player, float multiplier = 1f)
    {
        int damage = Mathf.RoundToInt(upgradeLevels[upgradeLevel].damage * multiplier) + player.OutgoingDamageBonus;
        enemy.TakeDamage(damage, true);
    }
}

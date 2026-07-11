using UnityEngine;

[CreateAssetMenu(fileName = "Fireball", menuName = "Cards/Fireball")]
public class Fireball : Card
{
    // deals magic damage
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1f)
    {
        int damage = Mathf.RoundToInt(upgradeLevels[upgradeLevel].damage * multiplier) + player.OutgoingDamageBonus;
        DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage, true);
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "Smite", menuName = "Cards/Smite")]
public class Smite : Card
{
    // deals magic damage and secondary damage based on a percentage of the enemy's max health
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1f)
    {
        int damage = Mathf.RoundToInt(Damage * multiplier) + player.OutgoingDamageBonus;
        int secondaryDamage = Mathf.RoundToInt(enemy.GetMaxHealth() * Percentage1) + player.OutgoingDamageBonus;
        DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage + secondaryDamage, true, index);
    }
}

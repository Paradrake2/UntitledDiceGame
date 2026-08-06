using UnityEngine;

[CreateAssetMenu(fileName = "RitualBlast", menuName = "Cards2/RitualBlast")]
public class RitualBlast : Card
{
    // sacrifices 20% of health to deal massive magic damage to the enemy
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        int healthSacrifice = Mathf.CeilToInt(player.CurrentHealth * 0.15f);
        player.TakeDamage(healthSacrifice, true);
        int damageAmount = Mathf.CeilToInt(enemy.GetMaxHealth() * Percentage1);
        DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damageAmount, true, index);
    }
}

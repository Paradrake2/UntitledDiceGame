using UnityEngine;

[CreateAssetMenu(fileName = "Smite", menuName = "Cards/Smite")]
public class Smite : Card
{
    // deals both physical and magic damage, secondary dice gives boost to either magic or physical depending on even or odd roll
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1f)
    {
        int damage = Mathf.RoundToInt(Damage * multiplier) + player.OutgoingDamageBonus;
        DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage, true, index);
        int secondaryDamage = Mathf.RoundToInt(enemy.GetMaxHealth() * Percentage1) + player.OutgoingDamageBonus;
        DamageManager.Instance.ApplyDamageToEnemy(enemy, player, secondaryDamage, true, index);
    }
}

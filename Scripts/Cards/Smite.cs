using UnityEngine;

[CreateAssetMenu(fileName = "Smite", menuName = "Cards/Smite")]
public class Smite : Card
{
    // deals both physical and magic damage, secondary dice gives boost to either magic or physical depending on even or odd roll
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1f)
    {
        int damage = Mathf.RoundToInt(Damage * multiplier) + player.OutgoingDamageBonus;
        
        DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage / 2, false, index);
        DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage / 2, true, index);

        int roll = DiceManager.Instance.RollSecondaryDie();
        if (roll % 2 == 0)
        {
            // even roll, deal magic damage
            int magicDamage = Mathf.RoundToInt(Int1 * multiplier) + player.OutgoingDamageBonus;
            DamageManager.Instance.ApplyDamageToEnemy(enemy, player, magicDamage, true, index);
        }
        else
        {
            // odd roll, deal physical damage
            int physicalDamage = Mathf.RoundToInt(Int1 * multiplier) + player.OutgoingDamageBonus;
            DamageManager.Instance.ApplyDamageToEnemy(enemy, player, physicalDamage, false, index);
        }
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "WildStrike", menuName = "Cards/WildStrike")]
public class WildStrike : Card
{
    // deals damage in a wide range
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        // damage is max dmg, int1 is min dmg
        int finalDamage = (int)(Random.Range(Int1, Damage) * multiplier + player.OutgoingDamageBonus);
        DamageManager.Instance.ApplyDamageToEnemy(enemy, player, finalDamage, false, index);

    }
}

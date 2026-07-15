using UnityEngine;

[CreateAssetMenu(fileName = "Stab", menuName = "Cards/Stab")]
public class Stab : Card
{
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1f)
    {
        int damage = Mathf.RoundToInt(Damage * multiplier) + player.OutgoingDamageBonus;
        DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage, false);
    }
}

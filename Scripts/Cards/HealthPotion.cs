using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotion", menuName = "Cards/HealthPotion")]
public class HealthPotion : Card
{
    // heals player for large amount
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        player.Heal(Heal);
    }
}

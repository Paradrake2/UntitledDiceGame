using UnityEngine;

[CreateAssetMenu(fileName = "EvenEdge", menuName = "Cards/EvenEdge")]
public class EvenEdge : Card
{
    // if card is in even index, deals bonus damage
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1f)
    {
        int damage = Mathf.RoundToInt(upgradeLevels[upgradeLevel].damage * multiplier) + player.OutgoingDamageBonus;
        if (index % 2 == 0)
        {
            damage += upgradeLevels[upgradeLevel].int1; // bonus damage for even index
        }
        enemy.TakeDamage(damage, true);
    }
}

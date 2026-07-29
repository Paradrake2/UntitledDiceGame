using UnityEngine;

[CreateAssetMenu(fileName = "MegaShield", menuName = "Cards/MegaShield")]
public class MegaShield : Card
{
    // gives the player a large amount of shield
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        int shieldAmount = Mathf.RoundToInt(Shield * multiplier);
        player.AddShield(shieldAmount);
    }
}

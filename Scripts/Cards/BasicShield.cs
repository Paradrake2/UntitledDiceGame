using UnityEngine;

[CreateAssetMenu(fileName = "BasicShield", menuName = "Cards/Basic Shield")]
public class BasicShield : Card
{
    // gives player some shield
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        int shieldAmount = Mathf.RoundToInt(Shield * multiplier);
        player.AddShield(shieldAmount);
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "Coinbag", menuName = "Cards/Coinbag")]
public class Coinbag : Card
{
    // rolls secondary dice and adds coins to player based on the result
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        int coins = Mathf.RoundToInt(Int1 * multiplier);
        int roll = DiceManager.Instance.RollSecondaryDie();
        int finalCoins = coins * roll;
        player.AddCointsFlat(finalCoins);
    }
}

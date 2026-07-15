using UnityEngine;

[CreateAssetMenu(fileName = "CoinSmash", menuName = "Cards/CoinSmash")]
public class CoinSmash : Card
{
    // deals damage based on number of coins the player has
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        int coins = player.Coins;
        int maxDmg = Mathf.RoundToInt(Int1 * multiplier);
        int damage = Mathf.RoundToInt(coins * Percentage1 * multiplier + player.OutgoingDamageBonus);
        int finalDamage = Mathf.Max(maxDmg, damage);
        DamageManager.Instance.ApplyDamageToEnemy(enemy, player, finalDamage, false);
    }
}

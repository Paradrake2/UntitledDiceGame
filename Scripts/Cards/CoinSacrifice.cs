using UnityEngine;

[CreateAssetMenu(fileName = "CoinSacrifice", menuName = "Cards/CoinSacrifice")]
public class CoinSacrifice : Card
{
    // sacrifice some coins to heal and shield the player
    // int1 is coin cost
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        player.AddCoins(-Int1); // uses AddCoins instead of SpendCoins to allow it to go negative
        player.Heal(Heal);
        player.AddShield(Shield);
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "Doubleplay", menuName = "Cards/Doubleplay")]
public class Doubleplay : Card
{
    // rolls secondary dice, plays the card at index of result twice
    public override void PlayCard(Enemy enemy, Player player, int cardIndex, float multiplier = 1f)
    {
        int secondaryRoll = DiceManager.Instance.RollSecondaryDie();
        // Play the card at the index of the secondary roll twice
        for (int i = 0; i < 2; i++)
        {
            BattleCardManager.Instance.PlayCard(secondaryRoll, enemy, player); // Assuming cardIndex is 0-based
        }
    }
}

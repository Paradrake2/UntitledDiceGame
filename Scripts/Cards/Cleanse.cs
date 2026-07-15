using UnityEngine;

[CreateAssetMenu(fileName = "Cleanse", menuName = "Cards/Cleanse")]
public class Cleanse : Card
{
    // remove all debuffs from the player
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        throw new System.NotImplementedException();
    }
}

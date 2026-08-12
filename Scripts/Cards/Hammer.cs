using UnityEngine;

[CreateAssetMenu(fileName = "Hammer", menuName = "Cards2/Hammer")]
public class Hammer : Card
{
    // deals random damage, chance to apply shattered effect
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        throw new System.NotImplementedException();
    }
}

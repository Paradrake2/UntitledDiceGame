using UnityEngine;

[CreateAssetMenu(fileName = "ManaBlaster", menuName = "Cards/ManaBlaster")]
public class ManaBlaster : Card
{
    // deals magic damage x number of times based on secondary dice roll / 2
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        int diceRoll = DiceManager.Instance.RollSecondaryDie();
        int numberOfHits = Mathf.CeilToInt(diceRoll / 2f);
        for (int i = 0; i < numberOfHits; i++)
        {
            int damage = Mathf.RoundToInt(Damage * multiplier) + player.OutgoingDamageBonus;
            DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage, true, index);
        }
    }
}

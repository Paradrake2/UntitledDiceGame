using UnityEngine;

[CreateAssetMenu(fileName = "ArcaneGamble", menuName = "Cards/ArcaneGamble")]
public class ArcaneGamble : Card
{
    // deals damage based on secondary dice roll
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        int diceRoll = DiceManager.Instance.RollSecondaryDie();
        switch (diceRoll)
        {
            case 1:
                // deal small amount of damage
                int damage = Mathf.RoundToInt(Damage/3 * multiplier) + player.OutgoingDamageBonus;
                DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage, true, index);
                break;
            case 2:
                // more damage than before
                int damage1 = Mathf.RoundToInt(Damage/2 * multiplier) + player.OutgoingDamageBonus;
                DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage1, true, index);
                break;
            case 3:
                int damage2 = Mathf.RoundToInt(Damage * multiplier) + player.OutgoingDamageBonus;
                DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage2, true, index);
                break;
            case 4:
                int damage3 = Mathf.RoundToInt(Damage * 1.5f * multiplier) + player.OutgoingDamageBonus;
                DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage3, true, index);
                break;
            case 5:
                int damage4 = Mathf.RoundToInt(Damage * 2.5f * multiplier) + player.OutgoingDamageBonus;
                DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage4, true, index);
                break;
            case 6:
                int damage5 = Mathf.RoundToInt(Damage * 5 * multiplier) + player.OutgoingDamageBonus;
                DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage5, true, index);
                break;
            default:
                Debug.LogError("Invalid dice roll: " + diceRoll);
                break;
        }
    }
}

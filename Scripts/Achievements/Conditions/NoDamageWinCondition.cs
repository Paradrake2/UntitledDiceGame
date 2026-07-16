using UnityEngine;

/// <summary>
/// Unlocks when the player wins a battle without taking any damage.
/// </summary>
[CreateAssetMenu(fileName = "NoDamageWinCondition", menuName = "Achievements/Conditions/NoDamageWin")]
public class NoDamageWinCondition : AchievementCondition
{
    public override bool Evaluate(AchievementStats stats)
        => stats.BattleJustWon && stats.DamageTakenThisBattle == 0;
}

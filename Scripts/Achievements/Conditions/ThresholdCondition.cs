using UnityEngine;

/// <summary>
/// Unlocks when stats.GetStat(stat) >= target.
/// Covers the majority of achievements: damage thresholds, stage reached,
/// upgrade counts, games played, enemies defeated, n-of-a-kind dice, etc.
/// </summary>
[CreateAssetMenu(fileName = "ThresholdCondition", menuName = "Achievements/Conditions/Threshold")]
public class ThresholdCondition : AchievementCondition
{
    [SerializeField] private AchievementStat stat;
    [SerializeField] private int target;

    public override bool Evaluate(AchievementStats stats) => stats.GetStat(stat) >= target;
}

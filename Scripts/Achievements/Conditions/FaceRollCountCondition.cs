using UnityEngine;

/// <summary>
/// Unlocks when a specific die face has appeared (across all dice) at least
/// target times in total during the current stage/battle.
///
/// Example: face=1, target=10  →  "Roll the number 1 ten times in a single stage"
/// </summary>
[CreateAssetMenu(fileName = "FaceRollCountCondition", menuName = "Achievements/Conditions/FaceRollCount")]
public class FaceRollCountCondition : AchievementCondition
{
    [SerializeField] [Range(1, 6)] private int face   = 1;
    [SerializeField]               private int target = 5;

    public override bool Evaluate(AchievementStats stats)
    {
        int faceIndex = face - 1;
        if (faceIndex < 0 || faceIndex >= stats.FaceRollCountsThisStage.Length)
            return false;

        return stats.FaceRollCountsThisStage[faceIndex] >= target;
    }
}

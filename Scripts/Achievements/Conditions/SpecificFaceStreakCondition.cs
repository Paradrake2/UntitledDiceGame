using UnityEngine;

/// <summary>
/// Unlocks when a specific die face appears at least minCount times,
/// for requiredStreak consecutive dice finalizations in a row.
///
/// Example: face=6, minCount=3, requiredStreak=2  →  "Roll at least three 6's twice in a row"
///
/// The streak resets when the condition fails on a roll or when a new battle starts.
/// </summary>
[CreateAssetMenu(fileName = "SpecificFaceStreakCondition", menuName = "Achievements/Conditions/SpecificFaceStreak")]
public class SpecificFaceStreakCondition : AchievementCondition
{
    [SerializeField] [Range(1, 6)] private int face           = 6;
    [SerializeField]               private int minCount       = 3;
    [SerializeField]               private int requiredStreak = 2;

    // Runtime state — not serialized so it resets cleanly between sessions
    [System.NonSerialized] private int[]  lastProcessedDice;
    [System.NonSerialized] private int    currentStreak;

    public override bool Evaluate(AchievementStats stats)
    {
        if (stats.LastDiceValues == null || stats.LastDiceValues.Length == 0)
            return false;

        // Only update the streak counter once per new dice roll (reference check)
        if (!ReferenceEquals(stats.LastDiceValues, lastProcessedDice))
        {
            lastProcessedDice = stats.LastDiceValues;

            int faceIndex = face - 1;
            if (stats.FaceCounts[faceIndex] >= minCount)
                currentStreak++;
            else
                currentStreak = 0;
        }

        return currentStreak >= requiredStreak;
    }

    public override void ResetState()
    {
        lastProcessedDice = null;
        currentStreak     = 0;
    }
}

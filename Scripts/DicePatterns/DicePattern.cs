using UnityEngine;

/// <summary>
/// ScriptableObject that defines a special dice combination and the multipliers it grants.
/// Add concrete subclasses for each pattern type, then assign them to DiceManager.SpecialPatterns.
/// Patterns are evaluated in order — the first match wins.
/// </summary>
public abstract class DicePattern : ScriptableObject
{
    [SerializeField] private string patternName;
    [SerializeField] private string patternDescription;

    public string PatternName => patternName;
    public string PatternDescription => patternDescription;

    /// <summary>Returns true if the given dice roll matches this pattern.</summary>
    public abstract bool Matches(int[] diceValues);

    /// <summary>
    /// Given that this pattern matched, returns the multiplier for the specified face value.
    /// Called once per die, so different face values in the same roll can get different multipliers.
    /// </summary>
    public abstract float GetMultiplierForValue(int faceValue, int[] diceValues);
}

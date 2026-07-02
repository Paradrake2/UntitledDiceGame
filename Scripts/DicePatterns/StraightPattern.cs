using UnityEngine;

/// <summary>
/// Matches when all dice form a consecutive run (e.g. 1,2,3,4,5 or 2,3,4,5,6).
/// Every face value in the roll receives the same multiplier.
/// </summary>
[CreateAssetMenu(fileName = "StraightPattern", menuName = "Dice Patterns/Straight")]
public class StraightPattern : DicePattern
{
    [SerializeField] private float multiplier = 1.5f;

    public override bool Matches(int[] diceValues)
    {
        if (diceValues.Length < 2) return false;

        int[] sorted = (int[])diceValues.Clone();
        System.Array.Sort(sorted);

        for (int i = 0; i < sorted.Length - 1; i++)
        {
            if (sorted[i + 1] != sorted[i] + 1) return false;
        }
        return true;
    }

    public override float GetMultiplierForValue(int faceValue, int[] diceValues)
    {
        return multiplier;
    }
}

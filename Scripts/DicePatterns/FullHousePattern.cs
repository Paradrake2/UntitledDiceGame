using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Matches when the dice split into exactly one group of 3 and one group of 2 (e.g. 2,2,2,5,5).
/// The triple face value receives tripleMultiplier; the pair face value receives pairMultiplier.
/// </summary>
[CreateAssetMenu(fileName = "FullHousePattern", menuName = "Dice Patterns/Full House")]
public class FullHousePattern : DicePattern
{
    [SerializeField] private float tripleMultiplier = 1.4f;
    [SerializeField] private float pairMultiplier = 1.2f;

    public override bool Matches(int[] diceValues)
    {
        Dictionary<int, int> counts = CountValues(diceValues);
        if (counts.Count != 2) return false;

        bool hasTriple = false;
        bool hasPair = false;
        foreach (int count in counts.Values)
        {
            if (count == 3) hasTriple = true;
            else if (count == 2) hasPair = true;
        }
        return hasTriple && hasPair;
    }

    public override float GetMultiplierForValue(int faceValue, int[] diceValues)
    {
        int count = 0;
        foreach (int v in diceValues)
            if (v == faceValue) count++;

        return count == 3 ? tripleMultiplier : pairMultiplier;
    }

    private static Dictionary<int, int> CountValues(int[] diceValues)
    {
        var counts = new Dictionary<int, int>();
        foreach (int v in diceValues)
            counts[v] = counts.TryGetValue(v, out int c) ? c + 1 : 1;
        return counts;
    }
}

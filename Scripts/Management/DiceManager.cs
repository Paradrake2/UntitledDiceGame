using System;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public DiceManagerUI diceManagerUI;
    [SerializeField] private int diceCount = 3;
    [SerializeField] private int rerollsPerTurn = 3;

    /// <summary>
    /// Multiplier applied to a card's effect when N dice show the same face value.
    /// Index 1 = one matching die (no bonus), index 2 = pair, index 3 = triple, etc.
    /// </summary>
    [SerializeField] private float[] duplicateMultipliers = { 0f, 1f, 1.1f, 1.25f, 1.5f, 1.75f, 2f };

    /// <summary>
    /// Special combination patterns (e.g. Straight, Full House). Checked in order before the
    /// count-based table — the first pattern that matches the current roll takes priority.
    /// </summary>
    [SerializeField] private DicePattern[] specialPatterns;

    [SerializeField] private int[] diceValues;
    private int rerollsRemaining;
    private bool diceFinalized;

    public int DiceCount => diceValues != null ? diceValues.Length : diceCount;
    public int RerollsRemaining => rerollsRemaining;
    public bool DiceFinalized => diceFinalized;

    /// <summary>Fired when the player accepts the dice or exhausts all rerolls.</summary>
    public event Action<int[]> OnDiceFinalized;

    /// <summary>Roll all dice fresh at the start of the player's turn.</summary>
    public void StartRoll()
    {
        int extraDice    = UpgradeManager.Instance != null ? UpgradeManager.Instance.GetTotalInt(UpgradeType.ExtraDice)    : 0;
        int extraRerolls = UpgradeManager.Instance != null ? UpgradeManager.Instance.GetTotalInt(UpgradeType.ExtraRerolls) : 0;

        diceValues = new int[diceCount + extraDice];
        rerollsRemaining = rerollsPerTurn + extraRerolls;
        diceFinalized = false;

        for (int i = 0; i < diceValues.Length; i++)
            diceValues[i] = RollSingle();
    }

    /// <summary>Reroll one die by its 0-based index. Consumes one reroll opportunity.</summary>
    public void RerollDie(int index)
    {
        if (diceFinalized) return;
        if (rerollsRemaining <= 0) return;
        if (index < 0 || index >= diceValues.Length) return;

        diceValues[index] = RollSingle();
        rerollsRemaining--;

        if (rerollsRemaining <= 0)
            FinalizeDice();
    }

    /// <summary>Player chooses to stop rerolling and accept the current dice.</summary>
    public void AcceptDice()
    {
        if (diceFinalized) return;
        FinalizeDice();
    }

    /// <summary>Returns a copy of the current dice values.</summary>
    public int[] GetValues() => (int[])diceValues.Clone();

    /// <summary>
    /// Returns the multiplier for the given face value based on how many dice currently show it.
    /// Special patterns are checked first; falls back to the count-based duplicateMultipliers table.
    /// </summary>
    public float GetMultiplierForValue(int faceValue)
    {
        if (specialPatterns != null)
        {
            foreach (DicePattern pattern in specialPatterns)
            {
                if (pattern != null && pattern.Matches(diceValues))
                    return pattern.GetMultiplierForValue(faceValue, diceValues);
            }
        }

        int count = 0;
        foreach (int v in diceValues)
            if (v == faceValue) count++;

        int clampedCount = Mathf.Clamp(count, 0, duplicateMultipliers.Length - 1);
        return duplicateMultipliers[clampedCount];
    }

    private void FinalizeDice()
    {
        diceFinalized = true;
        OnDiceFinalized?.Invoke(GetValues());
        diceManagerUI.ClearDiceUI();
        
        Debug.Log("Dice finalized: " + string.Join(", ", diceValues));
    }
    public int GetDieValue(int index)
    {
        if (index < 0 || index >= diceCount)
        {
            Debug.LogError("Invalid die index: " + index);
            return -1; // Return an invalid value to indicate an error
        }
        return diceValues[index];
    }
    private int RollSingle() => UnityEngine.Random.Range(1, 7);
}

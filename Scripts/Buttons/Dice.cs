using UnityEngine;
using TMPro;
public class Dice : MonoBehaviour
{
    [SerializeField] private int value;
    [SerializeField] private int index;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private DiceManager diceManager;
    public void SetValue(int newValue)
    {
        value = newValue;
        if (valueText != null)
        {
            valueText.text = value.ToString();
        }
        // Update multiplier whenever the value changes
        UpdateMultiplier();
    }
    public void Initialize(int newIndex, DiceManager manager)
    {
        index = newIndex;
        diceManager = manager;
        
        // Subscribe to reroll events to update multipliers dynamically
        if (diceManager != null)
        {
            diceManager.OnDieRerolled += UpdateMultiplier;
            // Set the initial multiplier now that diceManager is available
            UpdateMultiplier();
        }
    }
    public void OnClick()
    {
        if (diceManager != null)
        {
            diceManager.RerollDie(index);
            SetValue(diceManager.GetDieValue(index));
            // Multiplier will be updated automatically via OnDieRerolled event
        }
    }
    public void SetMultiplier(float multiplier)
    {
        if (multiplierText != null)
        {
            multiplierText.text = "x" + multiplier.ToString("0.##");
        }
    }

    /// <summary>Updates the multiplier text based on the current die value and other dice on the board.</summary>
    public void UpdateMultiplier()
    {
        if (diceManager != null && multiplierText != null)
        {
            float multiplier = diceManager.GetMultiplierForValue(value);
            SetMultiplier(multiplier);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        if (diceManager != null)
        {
            diceManager.OnDieRerolled -= UpdateMultiplier;
        }
    }
}

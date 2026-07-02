using UnityEngine;
using TMPro;
public class Dice : MonoBehaviour
{
    [SerializeField] private int value;
    [SerializeField] private int index;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private DiceManager diceManager;
    public void SetValue(int newValue)
    {
        value = newValue;
        if (valueText != null)
        {
            valueText.text = value.ToString();
        }
    }
    public void Initialize(int newIndex, DiceManager manager)
    {
        index = newIndex;
        diceManager = manager;
    }
    public void OnClick()
    {
        if (diceManager != null)
        {
            diceManager.RerollDie(index);
            SetValue(diceManager.GetDieValue(index));
        }
    }
}

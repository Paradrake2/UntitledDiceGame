using UnityEngine;
using TMPro;
public class RollButton : MonoBehaviour
{
    [SerializeField] private DiceManager diceManager;
    [SerializeField] private DiceManagerUI diceManagerUI;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private bool hasRolled = false; // used to determine whether this is rolling or confirming the roll
    void OnEnable()
    {
        diceManager.OnDiceFinalized += SetHasRolled;
    }
    void OnDisable()
    {
        diceManager.OnDiceFinalized -= SetHasRolled;
    }
    public void SetHasRolled(int[] i)
    {
        hasRolled = false;
        buttonText.text = hasRolled ? "OK" : "Roll";
    }
    public void RollDice()
    {
        if (diceManager != null && !hasRolled)
        {
            diceManager.StartRoll();
            diceManagerUI.UpdateDiceUI(diceManager.GetValues());
            buttonText.text = "OK";
            hasRolled = true;
        }
        else if (diceManager != null && hasRolled)
        {
            diceManager.AcceptDice();
            //diceManagerUI.ClearDiceUI();
            buttonText.text = "Roll";
            hasRolled = false;
        }
        else
        {
            Debug.LogError("DiceManager not found in the scene.");
        }
    }
    void Start()
    {
        if (diceManager == null)
        {
            diceManager = FindAnyObjectByType<DiceManager>();
            if (diceManager == null)
            {
                Debug.LogError("DiceManager not found in the scene.");
            }
        }
    }

}

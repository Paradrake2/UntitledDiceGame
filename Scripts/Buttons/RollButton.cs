using UnityEngine;
using TMPro;
public class RollButton : MonoBehaviour
{
    [SerializeField] private DiceManager diceManager;
    [SerializeField] private DiceManagerUI diceManagerUI;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private bool hasRolled = false; // used to determine whether this is rolling or confirming the roll
    [SerializeField] private bool isEnabled = false; // used to determine whether the button is enabled or not
    void OnEnable()
    {
        if (diceManager != null)
        {
            diceManager.OnDiceFinalized += SetHasRolled;
        }

        var animationManager = AnimationManager.TryGetInstance();
        if (animationManager != null)
        {
            animationManager.TurnNumberAnimationCompleted += EnableButton;
        }
        // CombatManager.Instance.<event> += EnableButton; // enable the button when the intro animation is completed
    }
    void OnDisable()
    {
        if (diceManager != null)
        {
            diceManager.OnDiceFinalized -= SetHasRolled;
        }

        var animationManager = AnimationManager.TryGetInstance();
        if (animationManager != null)
        {
            animationManager.TurnNumberAnimationCompleted -= EnableButton;
        }
    }
    void EnableButton()
    {
        isEnabled = true;
    }
    void DisableButton()
    {
        isEnabled = false;
    }
    public void SetHasRolled(int[] i)
    {
        hasRolled = false;
        DisableButton();
        buttonText.text = hasRolled ? "OK" : "Roll";
    }
    public void RollDice()
    {
        if (!isEnabled) return; // do nothing if the button is disabled
        if (diceManager == null)
        {
            Debug.LogError("DiceManager not found in the scene.");
            return;
        }

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

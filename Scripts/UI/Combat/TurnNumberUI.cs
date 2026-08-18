using UnityEngine;
using TMPro;

public class TurnNumberUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private CombatManager combatManager;
    void OnEnable()
    {
        combatManager.NewTurnStarted += UpdateTurnNumber;
    }
    void OnDisable()
    {
        combatManager.NewTurnStarted -= UpdateTurnNumber;
    }
    public void UpdateTurnNumber(int turnNumber)
    {
        turnNumberText.text = $"{turnNumber + 1}";
    }
}

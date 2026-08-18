using UnityEngine;
using TMPro;
public class StageNumberUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageNumberText;
    void OnEnable()
    {
        CombatManager.Instance.StageIncreased += UpdateStageNumber;
    }
    void OnDisable()
    {
        CombatManager.Instance.StageIncreased -= UpdateStageNumber;
    }
    public void UpdateStageNumber(int stageNumber)
    {
        stageNumberText.text = $"{stageNumber}";
    }
}

using UnityEngine;

public class EnemyHealthGainedAnimation : ValueAnimation
{
    [SerializeField] private Transform anchor;
    private void OnEnable()
    {
        CombatManager.Instance.EnemyHealthHealed += HandleEnemyHealthGained;
    }
    private void OnDisable()
    {
        CombatManager.Instance.EnemyHealthHealed -= HandleEnemyHealthGained;
    }
    private void HandleEnemyHealthGained(int amount)
    {
        PlayAnimation(amount, anchor != null ? anchor : transform);
    }
}

using UnityEngine;

public class EnemyShieldGainedAnimation : ValueAnimation
{
    [SerializeField] private Transform anchor;
    private void OnEnable()
    {
        CombatManager.Instance.EnemyShieldHealed += HandleEnemyShieldGained;
    }
    private void OnDisable()
    {
        CombatManager.Instance.EnemyShieldHealed -= HandleEnemyShieldGained;
    }
    private void HandleEnemyShieldGained(int amount)
    {
        PlayAnimation(amount, anchor != null ? anchor : transform);
    }
}

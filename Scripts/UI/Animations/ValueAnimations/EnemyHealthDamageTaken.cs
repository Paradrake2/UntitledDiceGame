using UnityEngine;

public class EnemyHealthDamageTaken : ValueAnimation
{
    [SerializeField] private Transform anchor;
    private void OnEnable()
    {
        CombatManager.Instance.EnemyHealthDamageTaken += HandleEnemyHealthDamaged;
    }
    private void OnDisable()
    {
        CombatManager.Instance.EnemyHealthDamageTaken -= HandleEnemyHealthDamaged;
    }
    private void HandleEnemyHealthDamaged(int amount)
    {
        PlayAnimation(amount, anchor != null ? anchor : transform);
    }
}

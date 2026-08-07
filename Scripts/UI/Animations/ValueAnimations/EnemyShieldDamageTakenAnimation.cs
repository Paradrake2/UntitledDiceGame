using UnityEngine;

public class EnemyShieldDamageTakenAnimation : ValueAnimation
{
    [SerializeField] private Transform anchor;
    private void OnEnable()
    {
        CombatManager.Instance.EnemyShieldDamageTaken += HandleEnemyShieldDamaged;
    }
    private void OnDisable()
    {
        CombatManager.Instance.EnemyShieldDamageTaken -= HandleEnemyShieldDamaged;
    }
    private void HandleEnemyShieldDamaged(int amount)
    {
        PlayAnimation(amount, anchor != null ? anchor : transform);
    }
}


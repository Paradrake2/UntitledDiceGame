using UnityEngine;

public class PlayerShieldDamagedAnimation : ValueAnimation
{
    [SerializeField] private Transform anchor;
    private void OnEnable()
    {
        CombatManager.Instance.PlayerShieldDamageTaken += HandlePlayerShieldDamaged;
    }

    private void OnDisable()
    {
        CombatManager.Instance.PlayerShieldDamageTaken -= HandlePlayerShieldDamaged;
    }

    private void HandlePlayerShieldDamaged(int amount)
    {
        PlayAnimation(amount, anchor != null ? anchor : transform);
    }
}

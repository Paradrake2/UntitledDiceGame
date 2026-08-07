using UnityEngine;

public class PlayerShieldDamagedAnimation : ValueAnimation
{
    [SerializeField] private Transform anchor;
    private void OnEnable()
    {
        combatManager.PlayerShieldDamageTaken += HandlePlayerShieldDamaged;
    }

    private void OnDisable()
    {
        combatManager.PlayerShieldDamageTaken -= HandlePlayerShieldDamaged;
    }

    private void HandlePlayerShieldDamaged(int amount)
    {
        PlayAnimation(amount, anchor != null ? anchor : transform);
    }
}

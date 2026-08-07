using UnityEngine;

public class PlayerShieldGainedAnimation : ValueAnimation
{
    [SerializeField] private Transform anchor;
    private void OnEnable()
    {
        CombatManager.Instance.PlayerShieldHealed += HandlePlayerShieldGained;
    }

    private void OnDisable()
    {
        CombatManager.Instance.PlayerShieldHealed -= HandlePlayerShieldGained;
    }

    private void HandlePlayerShieldGained(int amount)
    {
        PlayAnimation(amount, anchor != null ? anchor : transform);
    }
}

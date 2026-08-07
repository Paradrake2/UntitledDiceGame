using UnityEngine;

public class PlayerShieldGainedAnimation : ValueAnimation
{
    [SerializeField] private Transform anchor;
    private void OnEnable()
    {
        combatManager.PlayerShieldHealed += HandlePlayerShieldGained;
    }

    private void OnDisable()
    {
        combatManager.PlayerShieldHealed -= HandlePlayerShieldGained;
    }

    private void HandlePlayerShieldGained(int amount)
    {
        PlayAnimation(amount, anchor != null ? anchor : transform);
    }
}

using UnityEngine;

public class PlayerHealthGainedAnimation : ValueAnimation
{
    [SerializeField] private Transform anchor;
    private void OnEnable()
    {
        CombatManager.Instance.PlayerHealthHealed += HandlePlayerHealthGained;
    }

    private void OnDisable()
    {
        CombatManager.Instance.PlayerHealthHealed -= HandlePlayerHealthGained;
    }

    private void HandlePlayerHealthGained(int amount)
    {
        PlayAnimation(amount, anchor != null ? anchor : transform);
    }
}

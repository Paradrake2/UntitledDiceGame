using UnityEngine;

public class PlayerHealthGainedAnimation : ValueAnimation
{
    [SerializeField] private Transform anchor;
    private void OnEnable()
    {
        combatManager.PlayerHealthHealed += HandlePlayerHealthGained;
    }

    private void OnDisable()
    {
        combatManager.PlayerHealthHealed -= HandlePlayerHealthGained;
    }

    private void HandlePlayerHealthGained(int amount)
    {
        PlayAnimation(amount, anchor != null ? anchor : transform);
    }
}

using UnityEngine;

public class PlayerHealthDamageTakenAnimation : ValueAnimation
{
    [SerializeField] private Transform anchor;

    private void OnEnable()
    {
        CombatManager.Instance.PlayerHealthDamageTaken += HandlePlayerDamageTaken;
    }

    private void OnDisable()
    {
        CombatManager.Instance.PlayerHealthDamageTaken -= HandlePlayerDamageTaken;
    }

    private void HandlePlayerDamageTaken(int amount)
    {
        PlayAnimation(amount, anchor != null ? anchor : transform);
    }
}

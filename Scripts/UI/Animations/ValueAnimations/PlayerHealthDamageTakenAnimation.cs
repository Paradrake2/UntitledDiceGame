using UnityEngine;

public class PlayerHealthDamageTakenAnimation : ValueAnimation
{
    [SerializeField] private Transform anchor;

    private void OnEnable()
    {
        combatManager.PlayerHealthDamageTaken += HandlePlayerDamageTaken;
    }

    private void OnDisable()
    {
        combatManager.PlayerHealthDamageTaken -= HandlePlayerDamageTaken;
    }

    private void HandlePlayerDamageTaken(int amount)
    {
        PlayAnimation(amount, anchor != null ? anchor : transform);
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "Weaken", menuName = "Cards/Weaken")]
public class Weaken : Card
{
    public StatusEffect statusEffect;
    // weakens enemy, reducing their damage for a few turns based on upgrade level
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        enemy.StatusEffects.AddEffect(statusEffect, Int1);
        CombatManager.Instance.NotifyEnemyStatusEffectApplied(statusEffect);
    }
}

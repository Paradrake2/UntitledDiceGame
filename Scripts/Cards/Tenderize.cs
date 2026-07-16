using UnityEngine;

[CreateAssetMenu(fileName = "Tenderize", menuName = "Cards/Tenderize")]
public class Tenderize : Card
{
    public StatusEffect statusEffect;
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        enemy.StatusEffects.AddEffect(statusEffect, Int1);
        CombatManager.Instance.NotifyEnemyStatusEffectApplied(statusEffect);
    }
    // adds a status effect to the enemy that increases the physical damage they take for a few turns based on upgrade level
}

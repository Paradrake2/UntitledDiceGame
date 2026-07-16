using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all active status effects on a single combatant (Player or Enemy).
/// Call TriggerEffects at the appropriate points in the turn, and ModifyOutgoingDamage
/// before dealing physical or magic damage.
/// </summary>
public class StatusEffectHandler
{
    private readonly List<StatusEffectInstance> activeEffects = new List<StatusEffectInstance>();

    public IReadOnlyList<StatusEffectInstance> ActiveEffects => activeEffects;

    /// <summary>
    /// Adds a new instance of the given effect to this combatant.
    /// If the effect is not stackable and is already active, extends the duration instead.
    /// </summary>
    public void AddEffect(StatusEffect effect, int duration)
    {
        if (!effect.Stackable)
        {
            StatusEffectInstance existing = activeEffects.Find(i => i.Effect.GetType() == effect.GetType());
            if (existing != null)
            {
                existing.ExtendDuration();
                return;
            }
        }
        activeEffects.Add(new StatusEffectInstance(effect, duration));
        Debug.Log($"Added effect: {effect.name}, duration: {effect.duration}");
    }

    /// <summary>
    /// Fires all active effects whose trigger matches, then expires any that have run out of duration.
    /// Use for StartOfTurn and EndOfTurn triggers.
    /// </summary>
    public void TriggerEffects(StatusEffectTrigger trigger, StatusEffectContext ctx)
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            StatusEffectInstance instance = activeEffects[i];
            if (instance.Effect.Trigger != trigger) continue;

            instance.Effect.OnTrigger(ctx);
            if (instance.DecrementDuration())
                activeEffects.RemoveAt(i);
            Debug.Log($"Triggered effect: {instance.Effect.name}, remaining duration: {instance.RemainingDuration}");
            CombatManager.Instance.NotifyStatusEffectTriggered(instance.Effect, ctx.IsPlayerEffect);
        }
    }

    /// <summary>
    /// Applies all active OnPhysicalAttack / OnMagicAttack modifier effects to an outgoing damage
    /// value and consumes each one that fires. Call this inside a card before calling TakeDamage.
    /// </summary>
    public int ModifyOutgoingDamage(int damage, bool isMagic, StatusEffectContext ctx)
    {
        StatusEffectTrigger relevantTrigger = isMagic
            ? StatusEffectTrigger.OnMagicAttack
            : StatusEffectTrigger.OnPhysicalAttack;

        // Iterate over a copy to safely handle removals during the callback
        var effectsCopy = new List<StatusEffectInstance>(activeEffects);
        for (int i = effectsCopy.Count - 1; i >= 0; i--)
        {
            StatusEffectInstance instance = effectsCopy[i];
            if (instance.Effect.Trigger != relevantTrigger) continue;
            if (!activeEffects.Contains(instance))
                continue;

            damage = instance.Effect.ModifyOutgoingDamage(damage, isMagic, ctx);
            if (instance.DecrementDuration() && activeEffects.Contains(instance))
                activeEffects.RemoveAt(activeEffects.IndexOf(instance));
            CombatManager.Instance.NotifyStatusEffectTriggered(instance.Effect, ctx.IsPlayerEffect);
        }
        return damage;
    }
    public int ModifyIncomingDamage(int damage, bool isMagic, StatusEffectContext ctx)
    {
        // Iterate over a copy to safely handle removals during the callback
        var effectsCopy = new List<StatusEffectInstance>(activeEffects);
        for (int i = effectsCopy.Count - 1; i >= 0; i--)
        {
            StatusEffectInstance instance = effectsCopy[i];
            if (instance.Effect.Trigger != StatusEffectTrigger.OnReceiveDamage) continue;
            // Only process if the effect is still in the active list
            if (!activeEffects.Contains(instance))
                continue;

            damage = instance.Effect.ModifyIncomingDamage(damage, isMagic, ctx);
            if (instance.DecrementDuration() && activeEffects.Contains(instance))
                activeEffects.Remove(instance);
            CombatManager.Instance.NotifyStatusEffectTriggered(instance.Effect, ctx.IsPlayerEffect);
        }
        Debug.Log("Modified incoming damage: " + damage);
        return damage;
    }
    /// <summary>
    /// Returns true and removes the effect if a SkipTurn effect is active.
    /// Call at the very start of a turn to check whether it should be skipped.
    /// </summary>
    public bool ConsumeSkipTurn()
    {
        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (activeEffects[i].Effect.Trigger == StatusEffectTrigger.SkipTurn)
            {
                activeEffects.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
    public void RemoveEffect(StatusEffect effect)
    {
        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (activeEffects[i].Effect == effect)
            {
                activeEffects.RemoveAt(i);
                return;
            }
        }
    }
    public void DecreaseDurationOfAllEffects()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            StatusEffectInstance instance = activeEffects[i];
            if (instance.DecrementDuration())
                activeEffects.RemoveAt(i);
        }
    }
    public void SortEffectsByDuration()
    {
        activeEffects.Sort((a, b) => a.RemainingDuration.CompareTo(b.RemainingDuration));
    }
    /// <summary>Removes all active effects. Call when a battle ends or resets.</summary>
    public void Clear()
    {
        activeEffects.Clear();
    }
}

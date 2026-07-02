using System.Collections.Generic;

/// <summary>
/// Manages all active status effects on a single combatant (Player or Enemy).
/// Call TriggerEffects at the appropriate points in the turn, and ModifyOutgoingDamage
/// before dealing physical or magic damage.
/// </summary>
public class StatusEffectHandler
{
    private readonly List<StatusEffectInstance> activeEffects = new List<StatusEffectInstance>();

    public IReadOnlyList<StatusEffectInstance> ActiveEffects => activeEffects;

    /// <summary>Adds a new instance of the given effect to this combatant.</summary>
    public void AddEffect(StatusEffect effect)
    {
        activeEffects.Add(new StatusEffectInstance(effect));
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

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            StatusEffectInstance instance = activeEffects[i];
            if (instance.Effect.Trigger != relevantTrigger) continue;

            damage = instance.Effect.ModifyOutgoingDamage(damage, isMagic, ctx);
            if (instance.DecrementDuration())
                activeEffects.RemoveAt(i);
        }
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

    /// <summary>Removes all active effects. Call when a battle ends or resets.</summary>
    public void Clear()
    {
        activeEffects.Clear();
    }
}

using UnityEngine;

public enum SpecialEffectTrigger
{
    StartOfBattle,
    StartOfTurn,
    EndOfTurn,
    AfterNTurns,
    PlayerTurn,
    OnDamageTaken,
    OnDamageDealt,
}

// Abstract — use [CreateAssetMenu] on concrete subclasses, not here.
public abstract class SpecialEffect : ScriptableObject
{
    [SerializeField] protected string effectName;
    [SerializeField] protected string effectDescription;
    [SerializeField] private SpecialEffectTrigger trigger;
    /// <summary>Only used when trigger is AfterNTurns. Effect fires once turn count reaches this value.</summary>
    [SerializeField] private int turnThreshold = 1;

    public bool ShouldTrigger(SpecialEffectTrigger checkTrigger, int turnNumber)
    {
        if (trigger != checkTrigger) return false;
        if (trigger == SpecialEffectTrigger.AfterNTurns)
            return turnNumber >= turnThreshold;
        return true;
    }

    public virtual void ApplyEffect(SpecialEffectContext context) { }
    public virtual void ModifyIncomingDamage(DamageContext context) { }
    public virtual void ModifyOutgoingDamage(DamageContext context) { }
    public virtual void ModifyEnemyHealing(Enemy enemy, int amount) { }
}

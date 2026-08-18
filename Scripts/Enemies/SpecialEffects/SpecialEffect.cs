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
    EndOfBattle,
    OnDebuffed
}

public abstract class SpecialEffect : ScriptableObject
{
    [SerializeField] protected string effectName;
    [SerializeField] protected string effectDescription;
    [SerializeField] private SpecialEffectTrigger trigger;
    /// <summary>Only used when trigger is AfterNTurns. Effect fires once turn count reaches this value.</summary>
    [SerializeField] protected int turnThreshold = 1;
    public string EffectName => effectName;
    public string EffectDescription => effectDescription;
    public bool ShouldTrigger(SpecialEffectTrigger checkTrigger, int turnNumber)
    {
        if (trigger != checkTrigger) return false;
        if (trigger == SpecialEffectTrigger.AfterNTurns)
            return turnNumber >= turnThreshold;
        return true;
    }

    public virtual void ApplyEffect(SpecialEffectContext context) { }
    public virtual void ModifyIncomingDamage(DamageContext context) { }
    public virtual bool TryNegateIncomingDamage(DamageContext context) { return false; }
    public virtual void ResetRuntimeState() { }
    public virtual void ModifyOutgoingDamage(DamageContext context) { }
    public virtual void ModifyEnemyHealing(Enemy enemy, int amount) { }
    public virtual bool TryNegateDebuff(SpecialEffectContext context) { return false; }
}

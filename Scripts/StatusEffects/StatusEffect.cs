using UnityEngine;

/// <summary>
/// ScriptableObject template for a status effect. Create concrete subclasses for each effect type.
/// </summary>
public abstract class StatusEffect : ScriptableObject
{
    [SerializeField] private string effectName;
    [SerializeField] private string effectDescription;
    public virtual int duration { get; }

    public string EffectName => effectName;
    public string EffectDescription => effectDescription;

    /// <summary>Which game moment causes this effect to fire or be consumed.</summary>
    public abstract StatusEffectTrigger Trigger { get; }

    /// <summary>
    /// Called when the trigger fires (StartOfTurn, EndOfTurn).
    /// OnPhysicalAttack / OnMagicAttack effects are handled via ModifyOutgoingDamage instead.
    /// SkipTurn effects are handled by ConsumeSkipTurn and do not use this method.
    /// </summary>
    public abstract void OnTrigger(StatusEffectContext ctx);

    /// <summary>
    /// Override to modify an outgoing damage value before it is applied (e.g., DamagePotion).
    /// Only called for effects with Trigger == OnPhysicalAttack or OnMagicAttack.
    /// </summary>
    public virtual int ModifyOutgoingDamage(int damage, bool isMagic, StatusEffectContext ctx) => damage;
}

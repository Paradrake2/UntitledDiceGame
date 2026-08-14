using UnityEngine;

public enum TypeOfEffect
{
    Buff,
    Debuff
}

/// <summary>
/// ScriptableObject template for a status effect. Create concrete subclasses for each effect type.
/// </summary>
public abstract class StatusEffect : ScriptableObject
{
    [SerializeField] private string effectName;
    [SerializeField] private Sprite effectIcon;
    [SerializeField] private string effectDescription;
    [SerializeField] private bool stackable = false;
    [SerializeField] protected int effectDuration = 1;
    [SerializeField] private TypeOfEffect typeOfEffect = TypeOfEffect.Buff;
    [SerializeField] private bool isPermanent = false;
    public virtual int duration => effectDuration;
    public virtual bool Stackable => stackable;
    public Sprite EffectIcon => effectIcon;
    public TypeOfEffect TypeOfEffect => typeOfEffect;

    public string EffectName => effectName;
    public string EffectDescription => effectDescription;
    public bool IsPermanent => isPermanent;

    /// <summary>Which game moment causes this effect to fire or be consumed.</summary>
    public abstract StatusEffectTrigger Trigger { get; }

    // Called when the trigger fires (StartOfTurn, EndOfTurn).
    // OnPhysicalAttack, OnMagicAttack, and OnDealDamage effects are handled via ModifyOutgoingDamage instead.
    // SkipTurn effects are handled by ConsumeSkipTurn and do not use this method.
    public abstract void OnTrigger(StatusEffectContext ctx);


    // Override to modify an outgoing damage value before it is applied (e.g., DamagePotion).
    // Only called for effects with Trigger == OnPhysicalAttack, OnMagicAttack, or OnDealDamage.

    public virtual int ModifyOutgoingDamage(int damage, bool isMagic, StatusEffectContext ctx) => damage;
    public virtual int ModifyOutgoingDamage(int damage, bool isMagic, StatusEffectContext ctx, int remainingDuration)
        => ModifyOutgoingDamage(damage, isMagic, ctx);
    public virtual int ModifyIncomingDamage(int damage, bool isMagic, StatusEffectContext ctx) => damage;
}

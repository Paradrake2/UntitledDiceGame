using UnityEngine;

[CreateAssetMenu(fileName = "DamagePotionEffect", menuName = "Status Effects/Damage Potion")]
public class DamagePotionEffect : StatusEffect
{
    [SerializeField] private float damageMultiplier = 1.5f;

    public override StatusEffectTrigger Trigger => StatusEffectTrigger.OnPhysicalAttack;

    // Logic is handled entirely through ModifyOutgoingDamage.
    public override void OnTrigger(StatusEffectContext ctx) { }

    public override int ModifyOutgoingDamage(int damage, bool isMagic, StatusEffectContext ctx)
    {
        if (isMagic) return damage;
        return Mathf.RoundToInt(damage * damageMultiplier);
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "WeakenEffect", menuName = "Status Effects/Weaken")]
public class WeakenEffect : StatusEffect
{
    // applies a weaken effect that reduces the holder's physical damage by 50% for the duration of the effect, cannot stack with itself
    public override StatusEffectTrigger Trigger => StatusEffectTrigger.OnDealDamage;

    public override void OnTrigger(StatusEffectContext ctx) {}
    public override int ModifyOutgoingDamage(int damage, bool isMagic, StatusEffectContext ctx)
    {
        if (isMagic) return damage;
        return Mathf.RoundToInt(damage * 0.5f);
    }
}

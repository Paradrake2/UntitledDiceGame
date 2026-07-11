using UnityEngine;

[CreateAssetMenu(fileName = "TenderizeEffect", menuName = "Status Effects/Tenderize")]
public class TenderizeEffect : StatusEffect
{
    public override StatusEffectTrigger Trigger => StatusEffectTrigger.OnReceiveDamage;

    public override void OnTrigger(StatusEffectContext ctx) { }
    public override int ModifyIncomingDamage(int damage, bool isMagic, StatusEffectContext ctx)
    {
        if (isMagic) return damage;
        return Mathf.RoundToInt(damage * 1.5f);
    }
}

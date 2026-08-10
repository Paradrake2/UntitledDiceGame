using UnityEngine;

[CreateAssetMenu(fileName = "EmpoweredEffect", menuName = "Status Effects/Empowered")]
public class EmpoweredEffect : StatusEffect
{
    public override StatusEffectTrigger Trigger => StatusEffectTrigger.OnDealDamage;

    public override void OnTrigger(StatusEffectContext ctx){ }

    public override int ModifyOutgoingDamage(int damage, bool isMagic, StatusEffectContext ctx, int remainingDuration)
    {
        if (remainingDuration <= 0)
            remainingDuration = 1;

        return Mathf.RoundToInt(damage * (1f + remainingDuration * 0.1f));
    }
}

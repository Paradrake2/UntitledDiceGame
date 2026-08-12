using UnityEngine;

[CreateAssetMenu(fileName = "RecoveryBlockEffect", menuName = "Status Effects/Recovery Block Effect")]
public class RecoveryBlockEffect : StatusEffect
{
    public override StatusEffectTrigger Trigger => StatusEffectTrigger.OnHeal;

    public override void OnTrigger(StatusEffectContext ctx) { }
}

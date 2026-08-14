using UnityEngine;

[CreateAssetMenu(fileName = "ShatteredEffect", menuName = "Status Effects/Shattered Effect")]
public class ShatteredEffect : StatusEffect
{
    // reduce amount of shield gained by 70%
    public override StatusEffectTrigger Trigger => StatusEffectTrigger.OnReceiveShield;
    public override void OnTrigger(StatusEffectContext ctx){ }
}

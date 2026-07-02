using UnityEngine;

[CreateAssetMenu(fileName = "StunEffect", menuName = "Status Effects/Stun")]
public class StunEffect : StatusEffect
{
    public override StatusEffectTrigger Trigger => StatusEffectTrigger.SkipTurn;

    // Consumed by StatusEffectHandler.ConsumeSkipTurn(); OnTrigger is not used.
    public override void OnTrigger(StatusEffectContext ctx) { }
}

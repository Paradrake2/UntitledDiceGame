using UnityEngine;

[CreateAssetMenu(fileName = "PoisonEffect", menuName = "Status Effects/Poison Effect")]
public class PoisonEffect : StatusEffect
{
    public override StatusEffectTrigger Trigger => StatusEffectTrigger.StartOfTurn;

    public override void OnTrigger(StatusEffectContext ctx)
    {
        int poisonDamage = Mathf.RoundToInt(ctx.Player.CurrentHealth * 0.1f); // 10% of current health
        ctx.Player.TakeDamage(poisonDamage, false); // false indicates that this damage is not magical
    }
}

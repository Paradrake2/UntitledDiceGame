using UnityEngine;

[CreateAssetMenu(fileName = "BurnEffect", menuName = "Status Effects/Burn")]
public class BurnEffect : StatusEffect
{
    // deals large amount of damage over a few turns, cannot stack with itself
    public override StatusEffectTrigger Trigger => StatusEffectTrigger.StartOfTurn;
    public override int duration => 4;
    public override void OnTrigger(StatusEffectContext ctx)
    {
        if (duration <= 0)
        {
            if (ctx.IsPlayerEffect)
            {
                ctx.Player.StatusEffects.RemoveEffect(this);
            }
            else
            {
                ctx.Enemy.StatusEffects.RemoveEffect(this);
            }
            return;
        }
        if (ctx.IsPlayerEffect)
        {
            int damage = Mathf.Max(1, Mathf.RoundToInt(ctx.Player.MaxHealth * 0.15f));
            ctx.Player.TakeDamage(damage, true);
        }
        else
        {
            int damage = Mathf.Max(1, Mathf.RoundToInt(ctx.Enemy.EnemyStats.maxHealth * 0.15f));
            ctx.Enemy.TakeDamage(damage, true);
        }
    }
}

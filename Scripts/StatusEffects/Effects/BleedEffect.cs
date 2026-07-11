using UnityEngine;

[CreateAssetMenu(fileName = "BleedEffect", menuName = "Status Effects/Bleed")]
public class BleedEffect : StatusEffect
{
    // applies a bleed effect that deals x% of the holder's max health every turn, can stack with itself
    public override StatusEffectTrigger Trigger => StatusEffectTrigger.StartOfTurn;

    public override void OnTrigger(StatusEffectContext ctx)
    {
        // Deals 5% of the holder's max HP as magic damage (bypasses shield).
        if (ctx.IsPlayerEffect)
        {
            int damage = Mathf.Max(1, Mathf.RoundToInt(ctx.Player.MaxHealth * 0.05f));
            ctx.Player.TakeDamage(damage, true);
        }
        else
        {
            int damage = Mathf.Max(1, Mathf.RoundToInt(ctx.Enemy.EnemyStats.maxHealth * 0.05f));
            ctx.Enemy.TakeDamage(damage, true);
        }
    }
}

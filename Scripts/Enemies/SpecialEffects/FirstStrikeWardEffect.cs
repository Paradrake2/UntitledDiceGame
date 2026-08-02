using UnityEngine;

[CreateAssetMenu(fileName = "FirstStrikeWardEffect", menuName = "Special Effects/First Strike Ward Effect")]
public class FirstStrikeWardEffect : SpecialEffect
{
    private bool hasBlockedFirstHit;

    public override bool TryNegateIncomingDamage(DamageContext context)
    {
        if (hasBlockedFirstHit || context == null || context.Enemy == null || context.Amount <= 0)
            return false;

        hasBlockedFirstHit = true;
        context.Amount = 0;
        Debug.Log($"{context.Enemy.EnemyName} absorbed the first instance of incoming damage.");
        return true;
    }

    public override void ResetRuntimeState()
    {
        hasBlockedFirstHit = false;
    }
}

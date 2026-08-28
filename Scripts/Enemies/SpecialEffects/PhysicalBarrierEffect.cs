using UnityEngine;

public class PhysicalBarrierEffect : SpecialEffect
{
    public float damageReductionPercentage = 0.5f; // 50% damage reduction
    public override void ModifyIncomingDamage(DamageContext context)
    {
        if (!context.IsMagic)
        {
            int reducedAmount = Mathf.RoundToInt(context.Amount * (1 - damageReductionPercentage));
            Debug.Log($"{context.Enemy.EnemyName} reduced incoming physical damage from {context.Amount} to {reducedAmount}.");
            context.Amount = reducedAmount;
        }
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "ArcaneBarrier", menuName = "Special Effects/Arcane Barrier")]
public class ArcaneBarrier : SpecialEffect
{
    // reduces magical damage taken by a percentage
    public float damageReductionPercentage = 0.5f; // 50% damage reduction
    public override void ModifyIncomingDamage(DamageContext context)
    {
        if (context.IsMagic)
        {
            int reducedAmount = Mathf.RoundToInt(context.Amount * (1 - damageReductionPercentage));
            Debug.Log($"{context.Enemy.EnemyName} reduced incoming magical damage from {context.Amount} to {reducedAmount}.");
            context.Amount = reducedAmount;
        }
    }
}

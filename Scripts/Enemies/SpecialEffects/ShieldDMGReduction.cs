using UnityEngine;

[CreateAssetMenu(fileName = "New Shield DMG Reduction", menuName = "Special Effects/Shield DMG Reduction")]
public class ShieldDMGReduction : SpecialEffect
{
    public float damageReductionPercentage = 0.3f;
    public override void ModifyIncomingDamage(DamageContext context)
    {
        if (!context.HasShield) return; // Only reduce damage if the enemy has a shield.
        int reducedAmount = Mathf.RoundToInt(context.Amount * (1f - damageReductionPercentage));
        context.Amount = reducedAmount;
    }
}

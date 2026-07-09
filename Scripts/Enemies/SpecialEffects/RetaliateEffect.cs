using UnityEngine;

[CreateAssetMenu(fileName = "RetaliateEffect", menuName = "Special Effects/Retaliate Effect")]
public class RetaliateEffect : SpecialEffect
{
    // whenever the shield takes damage, retaliate with x% of the damage to the shield
    public float retaliatePercentage = 0.5f; // Retaliate with 50% of the damage taken
    public override void ModifyIncomingDamage(DamageContext context)
    {
        if (context.HasShield && context.Amount > 0)
        {
            int retaliateDamage = Mathf.RoundToInt(context.Amount * retaliatePercentage); // Retaliate with x% of the damage taken
            context.Player.TakeDamage(retaliateDamage, false); // Deal physical damage back to the attacker
        }
    }
}

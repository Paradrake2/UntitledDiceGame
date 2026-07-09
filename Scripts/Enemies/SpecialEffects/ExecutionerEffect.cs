using UnityEngine;

[CreateAssetMenu(fileName = "ExecutionerEffect", menuName = "Special Effects/Executioner Effect")]
public class ExecutionerEffect : SpecialEffect
{
    // if player has x% health or less, gain y% damage increase
    public float healthThreshold = 0.3f; // 30% health
    public float damageIncreasePercentage = 0.5f; // 50% damage increase
    public override void ModifyOutgoingDamage(DamageContext context)
    {
        if (context.Player.CurrentHealth <= context.Player.MaxHealth * healthThreshold)
        {
            context.Amount = Mathf.RoundToInt(context.Amount * (1 + damageIncreasePercentage));
        }
    }
}

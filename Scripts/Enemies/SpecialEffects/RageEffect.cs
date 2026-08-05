using UnityEngine;

[CreateAssetMenu(fileName = "RageEffect", menuName = "Special Effects/Rage Effect")]
public class RageEffect : SpecialEffect
{
    // when below x% health, gain y% damage increase
    public float healthThreshold = 0.3f; // 30% health
    public float damageIncreaseMultiplier = 1.5f; // 50% more damage
    public override void ModifyOutgoingDamage(DamageContext context)
    {
        if (context.Enemy.CurrentHealth <= context.Enemy.GetMaxHealth() * healthThreshold)
        {
            context.Amount = Mathf.RoundToInt(context.Amount * damageIncreaseMultiplier);
        }
    }
}

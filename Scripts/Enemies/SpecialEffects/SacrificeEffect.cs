using UnityEngine;

[CreateAssetMenu(fileName = "SacrificeEffect", menuName = "Special Effects/Sacrifice Effect")]
public class SacrificeEffect : SpecialEffect
{
    public float sacrificePercentage = 0.1f; // 10% of current health sacrificed
    // sacrifices a portion of enemy's health to deal damage
    public override void ApplyEffect(SpecialEffectContext context)
    {
        int sacrificeAmount = Mathf.RoundToInt(context.enemy.CurrentHealth * sacrificePercentage); // sacrifice 10% of current health
        context.enemy.TakeDamage(sacrificeAmount, true); // ignores shield
        context.damageAttempted += sacrificeAmount; // add the sacrificed amount to the damage attempted
    }
}

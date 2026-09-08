using UnityEngine;

[CreateAssetMenu(fileName = "SacrificeEffect", menuName = "Special Effects/Sacrifice Effect")]
public class SacrificeEffect : SpecialEffect
{
    public float sacrificePercentage = 0.1f; // 10% of current health sacrificed

    public override void ModifyOutgoingDamage(DamageContext context)
    {
        if (context?.Enemy == null) return;

        int sacrificeAmount = Mathf.RoundToInt(context.Enemy.CurrentHealth * sacrificePercentage);
        if (sacrificeAmount <= 0) return;

        context.Amount += sacrificeAmount; // add the sacrificed amount to the outgoing damage
        context.Enemy.TakeDamage(sacrificeAmount, true); // ignores shieldD
        Debug.Log("Sacrifice amount: " + sacrificeAmount);
    }
}

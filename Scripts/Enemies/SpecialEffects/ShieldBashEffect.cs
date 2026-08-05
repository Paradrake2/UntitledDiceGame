using UnityEngine;

[CreateAssetMenu(fileName = "ShieldBashEffect", menuName = "Special Effects/Shield Bash Effect")]
public class ShieldBashEffect : SpecialEffect
{
    // deals 100% of the enemy's shield as damage to the player
    public override void ModifyOutgoingDamage(DamageContext context)
    {
        if (context.HasShield)
        {
            int shieldDamage = context.Enemy.CurrentShield;
            context.Amount += shieldDamage;
        }
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "BulwarkEffect", menuName = "Special Effects/Bulwark Effect")]
public class BulwarkEffect : SpecialEffect
{
    // cannot be debuffed while the shield is up
    public override void ApplyEffect(SpecialEffectContext context)
    {
        if (context.enemy.EnemyStats.HasShield)
        {
            
        }
    }
}

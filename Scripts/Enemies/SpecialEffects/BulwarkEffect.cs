using UnityEngine;

[CreateAssetMenu(fileName = "BulwarkEffect", menuName = "Special Effects/Bulwark Effect")]
public class BulwarkEffect : SpecialEffect
{
    // cannot be debuffed while the shield is up
    public override bool TryNegateDebuff(SpecialEffectContext context)
    {
        if (context.enemy.EnemyStats.HasShield)
        {
            return true;
        }
        return false;
    }
}

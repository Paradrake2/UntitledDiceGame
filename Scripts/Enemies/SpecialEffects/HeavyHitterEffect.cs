using UnityEngine;

[CreateAssetMenu(fileName = "HeavyHitterEffect", menuName = "Special Effects/Heavy Hitter Effect")]
public class HeavyHitterEffect : SpecialEffect
{
    // every x attacks deal y% more damage
    public int attackThreshold = 3; // ex: every 3 attacks
    public float damageIncreasePercentage = 0.5f; // 50% damage increase

    public override void ApplyEffect(SpecialEffectContext context)
    {
        if (context.turnNumber % attackThreshold == 0)
        {
            context.damageAttempted = Mathf.RoundToInt(context.damageAttempted * (1 + damageIncreasePercentage));
        }
    }
}

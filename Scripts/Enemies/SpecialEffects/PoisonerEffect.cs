using UnityEngine;

[CreateAssetMenu(fileName = "PoisonerEffect", menuName = "Special Effects/Poisoner Effect")]
public class PoisonerEffect : SpecialEffect
{
    // chance to apply poison on hit
    public float poisonChance = 0.5f; // 50% chance to apply
    public StatusEffect statusEffect;
    public override void ApplyEffect(SpecialEffectContext context)
    {
        if (Random.value < poisonChance)
        {
            context.player.StatusEffects.AddEffect(statusEffect, 3); // apply poison for 3 turns
        }
    }
}

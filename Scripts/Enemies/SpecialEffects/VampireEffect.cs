using UnityEngine;

[CreateAssetMenu(fileName = "New Vampire Effect", menuName = "Special Effects/Vampire Effect")]
public class VampireEffect : SpecialEffect
{
    public float lifeStealPercentage = 0.5f; // 50% of damage dealt is returned as health
    //public override void ModifyOutgoingDamage(DamageContext context)
    //{
    //   int lifeStealAmount = Mathf.RoundToInt(context.Amount * lifeStealPercentage);
    //    context.Enemy?.Heal(lifeStealAmount);
    //}
    public override void ApplyEffect(SpecialEffectContext context)
    {
        int lifeStealAmount = Mathf.RoundToInt(context.damageTaken * lifeStealPercentage);
        context.enemy?.Heal(lifeStealAmount);
        Debug.Log($"{context.enemy.name} healed for {lifeStealAmount} health due to Vampire Effect.");
    }
}

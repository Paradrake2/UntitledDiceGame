using UnityEngine;

[CreateAssetMenu(fileName = "ConstantTrainingEffect", menuName = "Special Effects/Constant Training Effect")]
public class ConstantTrainingEffect : SpecialEffect
{
    // every attack increases damage by x%
    public float dmgIncreasePerAttack = 0.05f;
    private int attackCount = 0;
    public override void ApplyEffect(SpecialEffectContext context)
    {
        attackCount++;
        context.damageAttempted = Mathf.RoundToInt(context.damageAttempted * (1 + dmgIncreasePerAttack * attackCount));
    }
}

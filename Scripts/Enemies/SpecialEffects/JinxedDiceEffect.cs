using UnityEngine;

[CreateAssetMenu(fileName = "JinxedDiceEffect", menuName = "Special Effects/Jinxed Dice Effect")]
public class JinxedDiceEffect : SpecialEffect
{
    // takes reduced damage from certain dice numbers
    private int jinxedDiceNumber;
    public override void ModifyIncomingDamage(DamageContext context)
    {
        if (context.index.HasValue && context.index.Value == jinxedDiceNumber)
        {
            context.Amount = Mathf.RoundToInt(context.Amount * 0.33f);
        }
    }
    public override void ApplyEffect(SpecialEffectContext ctx)
    {
        // roll secondary dice
        // for the rest of the battle, the enemy takes only 33% damage from the rolled number
        DiceManager.Instance.RollSecondaryDie();
        jinxedDiceNumber = DiceManager.Instance.SecondaryDieValue;
        Debug.Log("Jinxed Dice Effect applied! Enemy will take reduced damage from dice number: " + jinxedDiceNumber);
    }
}

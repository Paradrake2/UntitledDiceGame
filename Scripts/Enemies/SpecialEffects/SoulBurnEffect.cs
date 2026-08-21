using UnityEngine;

[CreateAssetMenu(fileName = "Soul Burn Effect", menuName = "Special Effects/Soul Burn Effect")]
public class SoulBurnEffect : SpecialEffect
{
    // deal 5-10% of player's max hp as additional magic damage
    public float minPercentage = 0.05f; // 5%
    public float maxPercentage = 0.1f; // 10%
    public override void ModifyOutgoingDamage(DamageContext context)
    {
        int additionalDamage = Mathf.RoundToInt(context.Player.MaxHealth * Random.Range(minPercentage, maxPercentage));
        context.Amount += additionalDamage;
        Debug.LogWarning("Soul Burn Effect applied! Additional damage: " + additionalDamage);
    }
}

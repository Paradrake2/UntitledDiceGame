using UnityEngine;

[CreateAssetMenu(fileName = "GoldenGiantEffect", menuName = "Special Effects/Golden Giant Effect")]
public class GoldenGiantEffect : SpecialEffect
{
    // end battle after x turns, grant coins equival to how much damage the player dealt
    public int turnLimit = 10; // end battle after 10 turns
    public override void ApplyEffect(SpecialEffectContext context)
    {
        if (context.turnNumber >= turnLimit)
        {
            CombatManager.Instance.EndBattle();
        }
        int totalDamageDealt = context.enemy.GetMaxHealth() - context.enemy.CurrentHealth;
        float percentageOfMaxHealth = (float)totalDamageDealt / context.enemy.GetMaxHealth();
        int coinsToGrant = Mathf.RoundToInt(percentageOfMaxHealth * 100); // 100 coins is maximum reward
    }
}

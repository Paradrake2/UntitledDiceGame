using UnityEngine;

[CreateAssetMenu(fileName = "Stunner", menuName = "Cards2/Stunner")]
public class Stunner : Card
{
    // Deals medium physical damage and has a chance to stun
    [SerializeField] private StunEffect stunEffect;
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        int damageAmount = Mathf.CeilToInt(Damage * multiplier) + player.OutgoingDamageBonus;
        DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damageAmount, false, index);
        float chance = Random.Range(0f, 1f);
        if (chance <= Percentage1)
        {
            enemy.StatusEffects.AddEffect(stunEffect, 1);
            CombatManager.Instance.NotifyEnemyStatusEffectApplied(stunEffect);
        }
    }
}

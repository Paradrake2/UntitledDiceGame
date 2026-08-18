using UnityEngine;

[CreateAssetMenu(fileName = "Hammer", menuName = "Cards2/Hammer")]
public class Hammer : Card
{
    // deals low physical damage, chance to apply shattered effect
    [SerializeField] private ShatteredEffect shatteredEffect;
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        int damage = Mathf.RoundToInt(Damage * multiplier) + player.OutgoingDamageBonus;
        DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage, false, index);
        if (enemy.InflictDebuff(shatteredEffect, 2)) CombatManager.Instance.NotifyEnemyStatusEffectApplied(shatteredEffect);
    }
}

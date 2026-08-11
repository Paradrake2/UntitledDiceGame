using UnityEngine;

[CreateAssetMenu(fileName = "Cripple", menuName = "Cards2/Cripple")]
public class Cripple : Card
{
    // deal physical damage and weaken enemy, reducing damage
    [SerializeField] private WeakenEffect effect;
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        int damage = Mathf.RoundToInt(Damage * multiplier) + player.OutgoingDamageBonus;
        DamageManager.Instance.ApplyDamageToEnemy(enemy, player, damage, false, index);
        enemy.StatusEffects.AddEffect(effect, 1);
        CombatManager.Instance.NotifyEnemyStatusEffectApplied(effect);
    }
}

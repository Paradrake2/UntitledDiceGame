using UnityEngine;

[CreateAssetMenu(fileName = "HealingHorn", menuName = "Cards/HealingHorn")]
public class HealingHorn : Card
{
    [SerializeField] private DamagePotionEffect effect;
    // heals player for small amount and boosts damage for the rest of the turn 
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        int healAmount = Heal;
        player.Heal(healAmount);
        player.StatusEffects.AddEffect(effect, 1);
        CombatManager.Instance.NotifyPlayerStatusEffectApplied(effect);
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "HealingHorn", menuName = "Cards/HealingHorn")]
public class HealingHorn : Card
{
    [SerializeField] private DamagePotionEffect effect;
    // heals player for small amount and boosts damage for next attack
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        int healAmount = Heal;
        player.Heal(healAmount);
        player.StatusEffects.AddEffect(effect, Int1);
    }
}

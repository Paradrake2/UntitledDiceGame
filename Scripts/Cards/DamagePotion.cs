using UnityEngine;

[CreateAssetMenu(fileName = "DamagePotion", menuName = "Cards/DamagePotion")]
public class DamagePotion : Card
{
    [SerializeField] private DamagePotionEffect damagePotionEffect;

    // applies a damage boost effect to the player for the current turn
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1f)
    {
        player.StatusEffects.AddEffect(damagePotionEffect, Int1);
        CombatManager.Instance.NotifyPlayerStatusEffectApplied(damagePotionEffect);
    }
}

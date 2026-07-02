using UnityEngine;

[CreateAssetMenu(fileName = "DamagePotion", menuName = "Cards/DamagePotion")]
public class DamagePotion : Card
{
    [SerializeField] private DamagePotionEffect damagePotionEffect;

    public override void PlayCard(Enemy enemy, Player player, float multiplier = 1f)
    {
        player.StatusEffects.AddEffect(damagePotionEffect);
    }
}

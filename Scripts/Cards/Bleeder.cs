using UnityEngine;

[CreateAssetMenu(fileName = "Bleeder", menuName = "Cards/Bleeder")]
public class Bleeder : Card
{
    [SerializeField] private BleedEffect bleedEffect;
    // applies a bleed effect that deals x% of enemy's max health every turn
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1f)
    {
        enemy.StatusEffects.AddEffect(bleedEffect, Int1);
    }
}

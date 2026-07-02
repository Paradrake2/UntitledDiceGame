using UnityEngine;

[CreateAssetMenu(fileName = "Bleeder", menuName = "Cards/Bleeder")]
public class Bleeder : Card
{
    [SerializeField] private BleedEffect bleedEffect;

    public override void PlayCard(Enemy enemy, Player player, float multiplier = 1f)
    {
        enemy.StatusEffects.AddEffect(bleedEffect);
    }
}

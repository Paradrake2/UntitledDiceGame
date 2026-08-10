using UnityEngine;

[CreateAssetMenu(fileName = "Empower", menuName = "Cards2/Empower")]
public class Empower : Card
{
    [SerializeField] private EmpoweredEffect empoweredEffect;
    // every time this card is played, increase damage dealt by 10% for the rest of the battle and recover some health
    public override void PlayCard(Enemy enemy, Player player, int index, float multiplier = 1)
    {
        player.StatusEffects.AddEffect(empoweredEffect, 1);
        CombatManager.Instance.NotifyPlayerStatusEffectApplied(empoweredEffect);
        player.Heal(Mathf.CeilToInt(player.MaxHealth * Percentage1));
    }
}

/// <summary>
/// Passed to StatusEffect.OnTrigger so effects can read and modify either combatant.
/// IsPlayerEffect = true means the effect is attached to the player; false means the enemy.
/// </summary>
public class StatusEffectContext
{
    public Player Player { get; }
    public Enemy Enemy { get; }
    public bool IsPlayerEffect { get; }

    public StatusEffectContext(Player player, Enemy enemy, bool isPlayerEffect)
    {
        Player = player;
        Enemy = enemy;
        IsPlayerEffect = isPlayerEffect;
    }
}

using UnityEngine;

public class SpecialEffectContext
{
    public Enemy enemy;
    public Player player;
    public int turnNumber;
    public int damageAttempted;
    public int damageTaken;
    public bool isMagic;
    public SpecialEffectContext(Enemy enemy, Player player, int turnNumber, int damageAttempted, int damageTaken, bool isMagic)
    {
        this.enemy = enemy;
        this.player = player;
        this.turnNumber = turnNumber;
        this.damageAttempted = damageAttempted;
        this.damageTaken = damageTaken;
        this.isMagic = isMagic;
    }
}

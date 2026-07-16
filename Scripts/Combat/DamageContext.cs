using UnityEngine;

public class DamageContext
{
    public int Amount;
    public bool IsMagic;
    public bool HasShield;
    public Enemy Enemy;
    public Player Player;
    public int? index;
    public DamageContext(int amount, bool isMagic, bool hasShield, Enemy enemy, Player player, int? index)
    {
        Amount = amount;
        IsMagic = isMagic;
        HasShield = hasShield;
        Enemy = enemy;
        Player = player;
        this.index = index;
    }
}

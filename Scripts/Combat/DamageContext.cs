using UnityEngine;

public class DamageContext
{
    public int Amount;
    public bool IsMagic;
    public bool HasShield;
    public Enemy Enemy;
    public Player Player;
    public DamageContext(int amount, bool isMagic, bool hasShield, Enemy enemy, Player player)
    {
        Amount = amount;
        IsMagic = isMagic;
        HasShield = hasShield;
        Enemy = enemy;
        Player = player;
    }
}

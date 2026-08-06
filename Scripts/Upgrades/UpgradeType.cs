public enum UpgradeType
{
    OutgoingDamageBonus, // flat damage added to every attack
    DamageReduction,     // flat damage reduced from every hit received
    CoinBonus,           // extra coins added per reward
    ShopDiscount,        // percentage reduction on shop prices (0–1 range)
    ExtraDice,           // additional dice in the pool
    ExtraRerolls,        // additional rerolls per turn
    Revive,               // extra revive charges granted at the start of each run
    HealthBonus,         // flat health added to the player at the start of each run
    ShieldBonus,         // flat shield added to the player at the start of each run
    GemsBonus,            // extra gems added per run completion
    HealthRegen,          // percentage of health restored at the start of each turn
    ShieldRegen,          // flat shield restored at the start of each turn
    MaxUpgradeLevel,        // increases the maximum level of upgrades that can be purchased
    StartingCoins,          // increases the amount of coins the player starts with at the beginning of a run
    DiceCombinationBonus,    // increases dice combination effectiveness (e.g., doubles, triples, etc.)
    PhysicalDamageBonus,      // percentage increase to physical damage dealt
    MagicDamageBonus,         // percentage increase to magic damage dealt
    PhysicalDamageReduction,  // percentage reduction to physical damage received
    MagicDamageReduction,     // percentage reduction to magic damage received
    CriticalHitChance,         // percentage chance to deal a critical hit
    CriticalHitDamage,         // percentage increase to critical hit damage
    StunChance,                // percentage chance to stun enemies on hit
    BurnChance,                // percentage chance to burn enemies on hit
}

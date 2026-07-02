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
    HealthRegen,          // flat health restored at the start of each turn
    ShieldRegen,          // flat shield restored at the start of each turn
}

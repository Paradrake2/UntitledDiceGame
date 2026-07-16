/// <summary>
/// All numeric stats that ThresholdCondition can check.
/// </summary>
public enum AchievementStat
{
    // Per-turn
    PhysicalDamageDealtThisTurn,
    MagicalDamageDealtThisTurn,
    TotalDamageDealtThisTurn,
    ShieldGainedThisTurn,
    HealingReceivedThisTurn,
    CoinsGainedThisTurn,

    // Per-battle
    DamageTakenThisBattle,

    // Per-roll
    MaxDuplicateCountInRoll,

    // Per-run
    CurrentStage,
    UpgradesThisRun,
    MaxCardUpgradeLevelSeen,

    // All-time
    TotalGamesPlayed,
    TotalEnemiesDefeated,
}

/// <summary>
/// Accumulated runtime statistics consumed by AchievementConditions.
/// AchievementManager owns the single instance and keeps it up to date.
/// </summary>
public class AchievementStats
{
    // ── Per-Roll (updated on OnDiceFinalized) ─────────────────────────────────
    public int[] LastDiceValues = new int[0];
    public int   MaxDuplicateCount;          // highest count of any face in last roll
    public int[] FaceCounts = new int[6];    // index 0 = face 1 ... index 5 = face 6

    // ── Per-Turn (reset by PlayerTurnEnded) ───────────────────────────────────
    public int PhysicalDamageThisTurn;
    public int MagicalDamageThisTurn;
    public int ShieldGainedThisTurn;
    public int HealingReceivedThisTurn;
    public int CoinsGainedThisTurn;

    // ── Per-Battle (reset by BattleStarted) ───────────────────────────────────
    public int  DamageTakenThisBattle;
    public bool BattleJustWon;               // true only during the BattleWon evaluation pass

    // ── Per-Stage (reset by BattleStarted / StageIncreased) ───────────────────
    public int[] FaceRollCountsThisStage = new int[6]; // total dice showing each face this stage

    // ── Per-Run (reset by RunEnded) ───────────────────────────────────────────
    public int  CurrentStage;
    public int  UpgradesThisRun;
    public bool StartedWithAllUniqueCards;
    public int  MaxCardUpgradeLevelSeen;

    // ── All-Time (loaded from GameStatistics / PlayerPrefs) ───────────────────
    public int TotalGamesPlayed;
    public int TotalEnemiesDefeated;

    // ── Reset Helpers ─────────────────────────────────────────────────────────

    public void ResetPerTurn()
    {
        PhysicalDamageThisTurn  = 0;
        MagicalDamageThisTurn   = 0;
        ShieldGainedThisTurn    = 0;
        HealingReceivedThisTurn = 0;
        CoinsGainedThisTurn     = 0;
    }

    public void ResetPerBattle()
    {
        DamageTakenThisBattle = 0;
        BattleJustWon         = false;
    }

    public void ResetPerStage()
    {
        FaceRollCountsThisStage = new int[6];
    }

    public void ResetPerRun()
    {
        CurrentStage             = 1;
        UpgradesThisRun          = 0;
        StartedWithAllUniqueCards = false;
        MaxCardUpgradeLevelSeen  = 0;
    }

    // ── Stat Lookup (used by ThresholdCondition) ──────────────────────────────

    public int GetStat(AchievementStat stat)
    {
        return stat switch
        {
            AchievementStat.PhysicalDamageDealtThisTurn => PhysicalDamageThisTurn,
            AchievementStat.MagicalDamageDealtThisTurn  => MagicalDamageThisTurn,
            AchievementStat.TotalDamageDealtThisTurn    => PhysicalDamageThisTurn + MagicalDamageThisTurn,
            AchievementStat.ShieldGainedThisTurn        => ShieldGainedThisTurn,
            AchievementStat.HealingReceivedThisTurn     => HealingReceivedThisTurn,
            AchievementStat.CoinsGainedThisTurn         => CoinsGainedThisTurn,
            AchievementStat.DamageTakenThisBattle       => DamageTakenThisBattle,
            AchievementStat.CurrentStage                => CurrentStage,
            AchievementStat.UpgradesThisRun             => UpgradesThisRun,
            AchievementStat.TotalGamesPlayed            => TotalGamesPlayed,
            AchievementStat.TotalEnemiesDefeated        => TotalEnemiesDefeated,
            AchievementStat.MaxCardUpgradeLevelSeen     => MaxCardUpgradeLevelSeen,
            AchievementStat.MaxDuplicateCountInRoll     => MaxDuplicateCount,
            _ => 0
        };
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton orchestrator for the achievement system.
/// Subscribes to game events, maintains AchievementStats, evaluates all conditions,
/// and persists unlock state via PlayerPrefs.
/// </summary>
public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    [SerializeField] private Achievement[] achievements;

    private readonly HashSet<string>  unlockedIds = new HashSet<string>();
    private readonly AchievementStats stats       = new AchievementStats();

    private const string SavePrefix = "Achievement_Unlocked_";

    /// <summary>Fired when an achievement is unlocked for the first time.</summary>
    public event Action<Achievement> OnAchievementUnlocked;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadUnlocks();
    }

    private void Start()
    {
        // Sync persistent stats before subscribing
        if (GameStatistics.Instance != null)
        {
            stats.TotalGamesPlayed     = GameStatistics.Instance.TotalGamesPlayed;
            stats.TotalEnemiesDefeated = GameStatistics.Instance.TotalEnemiesDefeated;
        }
        stats.ResetPerRun();
        SubscribeToEvents();
    }

    private void OnDestroy() => UnsubscribeFromEvents();

    // ── Event Subscriptions ───────────────────────────────────────────────────

    private void SubscribeToEvents()
    {
        if (CombatManager.Instance != null)
        {
            CombatManager.Instance.BattleStarted       += OnBattleStarted;
            CombatManager.Instance.BattleWon           += OnBattleWon;
            CombatManager.Instance.RunEnded            += OnRunEnded;
            CombatManager.Instance.PlayerTurnEnded     += OnPlayerTurnEnded;
            CombatManager.Instance.StageIncreased      += OnStageIncreased;
            CombatManager.Instance.PlayerDamageTaken   += OnPlayerDamageTaken;
            CombatManager.Instance.PlayerHealingReceived += OnPlayerHealingReceived;
            CombatManager.Instance.PlayerShieldGained  += OnPlayerShieldGained;
            CombatManager.Instance.EnemyDamageDealt    += OnEnemyDamageDealt;
            CombatManager.Instance.PlayerCoinsGained   += OnCoinsGained;
        }

        if (DiceManager.Instance != null)
            DiceManager.Instance.OnDiceFinalized += OnDiceFinalized;

        ShopManager.OnCardUpgraded         += OnCardUpgraded;
        UpgradeManager.OnUpgradePurchased  += OnGemUpgradePurchased;
    }

    private void UnsubscribeFromEvents()
    {
        if (CombatManager.Instance != null)
        {
            CombatManager.Instance.BattleStarted         -= OnBattleStarted;
            CombatManager.Instance.BattleWon             -= OnBattleWon;
            CombatManager.Instance.RunEnded              -= OnRunEnded;
            CombatManager.Instance.PlayerTurnEnded       -= OnPlayerTurnEnded;
            CombatManager.Instance.StageIncreased        -= OnStageIncreased;
            CombatManager.Instance.PlayerDamageTaken     -= OnPlayerDamageTaken;
            CombatManager.Instance.PlayerHealingReceived -= OnPlayerHealingReceived;
            CombatManager.Instance.PlayerShieldGained    -= OnPlayerShieldGained;
            CombatManager.Instance.EnemyDamageDealt      -= OnEnemyDamageDealt;
            CombatManager.Instance.PlayerCoinsGained     -= OnCoinsGained;
        }

        if (DiceManager.Instance != null)
            DiceManager.Instance.OnDiceFinalized -= OnDiceFinalized;

        ShopManager.OnCardUpgraded        -= OnCardUpgraded;
        UpgradeManager.OnUpgradePurchased -= OnGemUpgradePurchased;
    }

    // ── Event Handlers ────────────────────────────────────────────────────────

    private void OnBattleStarted()
    {
        stats.ResetPerBattle();
        stats.ResetPerStage();
        stats.ResetPerTurn();
        ResetConditionStates();

        // Check "start with all unique cards" at the moment of battle start
        if (BattleCardManager.Instance != null)
        {
            var seen = new HashSet<Card>();
            bool allUnique = true;
            for (int i = 1; i <= 6; i++)
            {
                Card card = BattleCardManager.Instance.GetCard(i);
                if (card == null || !seen.Add(card)) { allUnique = false; break; }
            }
            stats.StartedWithAllUniqueCards = allUnique;
        }

        EvaluateAll();
    }

    private void OnBattleWon()
    {
        if (GameStatistics.Instance != null)
        {
            GameStatistics.Instance.RecordEnemyDefeated();
            stats.TotalEnemiesDefeated = GameStatistics.Instance.TotalEnemiesDefeated;
        }
        stats.BattleJustWon = true;
        EvaluateAll();
        stats.BattleJustWon = false;
    }

    private void OnRunEnded()
    {
        if (GameStatistics.Instance != null)
        {
            GameStatistics.Instance.RecordGamePlayed();
            stats.TotalGamesPlayed = GameStatistics.Instance.TotalGamesPlayed;
        }
        EvaluateAll();
        stats.ResetPerRun();
    }

    private void OnPlayerTurnEnded() => stats.ResetPerTurn();

    private void OnStageIncreased(int newStage)
    {
        stats.CurrentStage = newStage;
        stats.ResetPerStage();
        EvaluateAll();
    }

    private void OnPlayerDamageTaken(int amount)
    {
        stats.DamageTakenThisBattle += amount;
        EvaluateAll();
    }

    private void OnPlayerHealingReceived(int amount)
    {
        stats.HealingReceivedThisTurn += amount;
        EvaluateAll();
    }

    private void OnPlayerShieldGained(int amount)
    {
        stats.ShieldGainedThisTurn += amount;
        EvaluateAll();
    }

    private void OnEnemyDamageDealt(int amount, bool isMagic)
    {
        if (isMagic) stats.MagicalDamageThisTurn  += amount;
        else         stats.PhysicalDamageThisTurn += amount;
        EvaluateAll();
    }

    private void OnCoinsGained(int amount)
    {
        stats.CoinsGainedThisTurn += amount;
        EvaluateAll();
    }

    private void OnDiceFinalized(int[] values)
    {
        // Replace with a fresh array so reference equality detects new rolls in streak conditions
        stats.LastDiceValues = (int[])values.Clone();

        stats.FaceCounts = new int[6];
        int maxDup = 0;
        foreach (int v in values)
        {
            if (v < 1 || v > 6) continue;
            stats.FaceCounts[v - 1]++;
            if (stats.FaceCounts[v - 1] > maxDup) maxDup = stats.FaceCounts[v - 1];
        }
        stats.MaxDuplicateCount = maxDup;

        // Accumulate for the stage-wide face-roll-count tracking
        foreach (int v in values)
            if (v >= 1 && v <= 6)
                stats.FaceRollCountsThisStage[v - 1]++;

        EvaluateAll();
    }

    private void OnCardUpgraded(Card card)
    {
        if (card == null) return;
        stats.UpgradesThisRun++;
        if (card.UpgradeLevel > stats.MaxCardUpgradeLevelSeen)
            stats.MaxCardUpgradeLevelSeen = card.UpgradeLevel;
        EvaluateAll();
    }

    private void OnGemUpgradePurchased()
    {
        stats.UpgradesThisRun++;
        EvaluateAll();
    }

    // ── Evaluation ────────────────────────────────────────────────────────────

    private void EvaluateAll()
    {
        if (achievements == null) return;
        foreach (Achievement achievement in achievements)
        {
            if (achievement == null || achievement.Condition == null) continue;
            if (unlockedIds.Contains(achievement.AchievementId)) continue;
            if (achievement.Condition.Evaluate(stats))
                Unlock(achievement);
        }
    }

    private void ResetConditionStates()
    {
        if (achievements == null) return;
        foreach (Achievement achievement in achievements)
            achievement?.Condition?.ResetState();
    }

    private void Unlock(Achievement achievement)
    {
        unlockedIds.Add(achievement.AchievementId);
        PlayerPrefs.SetInt(SavePrefix + achievement.AchievementId, 1);
        PlayerPrefs.Save();
        OnAchievementUnlocked?.Invoke(achievement);
        Debug.Log($"[Achievements] Unlocked: {achievement.DisplayName}");
    }

    // ── Public API ────────────────────────────────────────────────────────────

    public bool IsUnlocked(Achievement achievement)
        => achievement != null && unlockedIds.Contains(achievement.AchievementId);

    // ── Persistence ───────────────────────────────────────────────────────────

    private void LoadUnlocks()
    {
        if (achievements == null) return;
        foreach (Achievement a in achievements)
        {
            if (a == null) continue;
            if (PlayerPrefs.GetInt(SavePrefix + a.AchievementId, 0) == 1)
                unlockedIds.Add(a.AchievementId);
        }
    }
}

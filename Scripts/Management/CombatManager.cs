using System.Collections;
using UnityEngine;
using System;
public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    [SerializeField] private Enemy currentEnemy;
    [SerializeField] private EnemyUI enemyUI;
    [SerializeField] private int currentStage = 1;
    [SerializeField] private Player player;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private DiceManager diceManager;
    [SerializeField] private BattleCardManager cardManager;
    [SerializeField] private CardManagerUI cardManagerUI;
    [SerializeField] private EnemyStatusEffectUI enemyStatusEffectUI;

    private int turnNumber;
    private bool battleActive;
    private int enemiesDefeated;
    [SerializeField] private int gemsPerEnemy = 5;

    public event Action<int> PlayerDamageTaken;
    public event Action<int> PlayerHealingReceived;
    public event Action<int> EnemyDamageTaken;
    public event Action<int> EnemyHealingReceived;
    public event Action<int> PlayerShieldGained;
    public event Action<int> EnemyShieldGained;
    public event Action<StatusEffect, bool> StatusEffectApplied; // called when player gains a status effect. True is player, false is enemy
    public event Action<StatusEffect, bool> StatusEffectRemoved; // called when player loses a status effect. True is player, false is enemy
    public event Action<StatusEffect, bool> StatusEffectTriggered; // called when a status effect is triggered. True is player, false is enemy
    public event Action<Enemy> EnemySelected;
    public event Action BattleStarted;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void StartBattle(int stage)
    {
        currentStage = stage;
        enemiesDefeated = 0;
        currentEnemy = DetermineEnemy();
        currentEnemy.ModifyStats(currentStage);
        currentEnemy.InitForBattle();
        player.InitForBattle();

        turnNumber = 0;
        EnemySelected?.Invoke(currentEnemy);
        battleActive = true;

        enemyUI.SetEnemy(currentEnemy);
        var context = new SpecialEffectContext(currentEnemy, player, turnNumber, damageAttempted: 0, damageTaken: 0, isMagic: false);
        currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.StartOfBattle, context);

        StartCoroutine(BattleLoop());
        BattleStarted?.Invoke();
    }

    /// <summary>Called by ShopManager after the shop closes to advance to the next stage.</summary>
    public void StartNextBattle()
    {
        IncreaseStage();
        StartBattle(currentStage);
    }

    private IEnumerator BattleLoop()
    {
        cardManagerUI?.RefreshUI();
        while (battleActive)
        {
            yield return StartCoroutine(RunPlayerTurn());

            if (!currentEnemy.IsAlive)
            {
                OnPlayerWon();
                yield break;
            }

            yield return StartCoroutine(RunEnemyTurn());

            if (!player.IsAlive)
            {
                OnPlayerLost();
                yield break;
            }
        }
    }
    
    private IEnumerator RunPlayerTurn()
    {
        var ctx = new StatusEffectContext(player, currentEnemy, isPlayerEffect: true);

        if (player.StatusEffects.ConsumeSkipTurn())
            yield break;

        player.StatusEffects.TriggerEffects(StatusEffectTrigger.StartOfTurn, ctx);
        playerUI.UpdateTexts();

        diceManager.StartRoll();

        // Wait until the player accepts the dice or runs out of rerolls.
        // The UI calls diceManager.RerollDie(index) or diceManager.AcceptDice().
        yield return new WaitUntil(() => diceManager.DiceFinalized);
        var context = new SpecialEffectContext(currentEnemy, player, turnNumber, damageAttempted: 0, damageTaken: 0, isMagic: false);
        currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.PlayerTurn, context);
        // Play cards in the order the dice appear left-to-right.
        // Each die value maps directly to a card position (1-6).
        int[] values = diceManager.GetValues();
        foreach (int value in values)
        {
            if (!currentEnemy.IsAlive) break;

            float multiplier = diceManager.GetMultiplierForValue(value);
            cardManager.PlayCard(value, currentEnemy, player, multiplier);
            enemyUI.UpdateTexts();
            yield return new WaitForSeconds(1f);
        }

        player.StatusEffects.TriggerEffects(StatusEffectTrigger.EndOfTurn, ctx);
        playerUI.UpdateTexts();
    }

    private IEnumerator RunEnemyTurn()
    {
        turnNumber++;
        diceManager.diceManagerUI.ClearDiceUI(); // clear dice
        var ctx = new StatusEffectContext(player, currentEnemy, isPlayerEffect: false);

        if (currentEnemy.StatusEffects.ConsumeSkipTurn())
        {
            yield return null;
            yield break;
        }

        currentEnemy.StatusEffects.TriggerEffects(StatusEffectTrigger.StartOfTurn, ctx);
        enemyUI.UpdateTexts();

        var context = new SpecialEffectContext(currentEnemy, player, turnNumber, damageAttempted: 0, damageTaken: 0, isMagic: false);
        currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.StartOfTurn, context);
        enemyUI.UpdateTexts();

        // Physical attacks - one per hit, each with a flash and a delay.
        for (int i = 0; i < currentEnemy.EnemyStats.physicalAttackAmount; i++)
        {
            enemyUI.FlashPhysicalDamageText();
            int damage = currentEnemy.EnemyStats.physicalAttackDamage;
            int damageTaken = player.TakeDamage(damage, false);
            PlayerDamageTaken?.Invoke(damage);
            var pcontext = new SpecialEffectContext(currentEnemy, player, turnNumber, damageAttempted: damage, damageTaken: damageTaken, isMagic: false);
            currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.OnDamageDealt, pcontext);

            playerUI.UpdateTexts();
            enemyUI.UpdateTexts();
            yield return new WaitForSeconds(1f);
        }

        // Magical attacks — one per hit, each with a flash and a delay.
        for (int i = 0; i < currentEnemy.EnemyStats.magicalAttackAmount; i++)
        {
            enemyUI.FlashMagicalDamageText();
            int damage = currentEnemy.EnemyStats.magicalAttackDamage;
            int damageTaken = player.TakeDamage(damage, true);
            PlayerDamageTaken?.Invoke(damageTaken);
            var mcontext = new SpecialEffectContext(currentEnemy, player, turnNumber, damageAttempted: damage, damageTaken: damageTaken, isMagic: true);
            currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.OnDamageDealt, mcontext);

            playerUI.UpdateTexts();
            enemyUI.UpdateTexts();
            yield return new WaitForSeconds(1f);
        }
        currentEnemy.Heal(currentEnemy.EnemyStats.healAmount);
        currentEnemy.AddShield(currentEnemy.EnemyStats.shieldAmount);
        var eotContext = new SpecialEffectContext(currentEnemy, player, turnNumber, damageAttempted: 0, damageTaken: 0, isMagic: false);
        currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.EndOfTurn, eotContext);
        enemyUI.UpdateTexts();
        currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.AfterNTurns, eotContext);
        enemyUI.UpdateTexts();

        currentEnemy.StatusEffects.TriggerEffects(StatusEffectTrigger.EndOfTurn, ctx);
        currentEnemy.IncreaseTurnStats(); // Increase enemy stats based on turn increases

        enemyUI.UpdateTexts();

        yield return null;
    }
    private void OnPlayerWon()
    {
        enemiesDefeated++;
        battleActive = false;
        player.AddCoins(currentEnemy.CoinReward);
        EndBattle();
    }

    private void OnPlayerLost()
    {
        if (player.TryRevive())
        {
            // Player had a revive charge — restore health and continue the run.
            playerUI.UpdateTexts();
            battleActive = true;
            StartCoroutine(BattleLoop());
            return;
        }

        // No revives left — award gems based on performance.
        int gemsEarned = enemiesDefeated * gemsPerEnemy;
        if (UpgradeManager.Instance != null)
            UpgradeManager.Instance.AddGems(gemsEarned);

        battleActive = false;
        EndBattle();
    }

    public void EndBattle()
    {
        ShopManager.Instance.OpenShop();
    }

    public void IncreaseStage()
    {
        currentStage++;
    }

    public void ResetStage()
    {
        currentStage = 1;
    }

    public Enemy DetermineEnemy()
    {
        // Choose which enemy to spawn from a pool based on current stage
        currentEnemy = EnemySelector.Instance.DetermineEnemy(currentStage);
        return currentEnemy;
    }
    public void Start()
    {
        //StartBattle(currentStage);
    }
    public void NotifyPlayerStatusEffectRemoved(StatusEffect effect)
    {
        StatusEffectRemoved?.Invoke(effect, true);
    }
    public void NotifyEnemyStatusEffectRemoved(StatusEffect effect)
    {
        StatusEffectRemoved?.Invoke(effect, false);
    }
    public void NotifyPlayerStatusEffectApplied(StatusEffect effect)
    {
        StatusEffectApplied?.Invoke(effect, true);
    }
    public void NotifyEnemyStatusEffectApplied(StatusEffect effect)
    {
        StatusEffectApplied?.Invoke(effect, false);
    }
    public void NotifyStatusEffectTriggered(StatusEffect effect, bool isPlayer)
    {
        Debug.Log($"Status effect triggered: {effect.name}, isPlayer: {isPlayer}");
        StatusEffectTriggered?.Invoke(effect, isPlayer);
    }
}

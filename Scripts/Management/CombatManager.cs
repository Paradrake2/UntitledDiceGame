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
    public event Action<int> NewTurnStarted;
    public event Action<StatusEffect, bool> StatusEffectApplied; // called when someone gains a status effect. True is player, false is enemy
    public event Action<StatusEffect, bool> StatusEffectRemoved; // called when someone loses a status effect. True is player, false is enemy
    public event Action<StatusEffect, bool> StatusEffectTriggered; // called when a status effect is triggered. True is player, false is enemy
    public event Action<Enemy> EnemySelected;
    public event Action BattleStarted;
    public event Action BattleEnded;
    public event Action BattleWon;               // fires only on player victory
    public event Action RunEnded;                // fires when the player has no revives left
    public event Action PlayerTurnEnded;         // fires at the end of the player's turn
    public event Action<int>      PlayerCoinsGained;
    public event Action<int>      StageIncreased;
    public event Action<int, bool> EnemyDamageDealt; // (amount, isMagic) — player dealing damage to enemy

    // UI events for damage/healing/shield changes. These are separate from the actual damage/healing methods to allow for UI animations to play without affecting the underlying game logic.
    public event Action<int> PlayerHealthDamageTaken; // for UI purposes
    public event Action<int> PlayerHealthHealed; // for UI purposes
    public event Action<int> PlayerShieldDamageTaken; // for UI purposes
    public event Action<int> PlayerShieldHealed; // for UI purposes
    public event Action<int> EnemyHealthDamageTaken; // for UI purposes
    public event Action<int> EnemyHealthHealed; // for UI purposes
    public event Action<int> EnemyShieldDamageTaken; // for UI purposes
    public event Action<int> EnemyShieldHealed; // for UI purposes
    public event Action<int> PlayerCoinsChanged; // for UI purposes
    public event Action EnemyPhysicalAttack;
    public event Action EnemyMagicalAttack;
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
        
        
        BattleStarted?.Invoke();
        StartCoroutine(BattleLoop());
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
        if (currentEnemy.SpecialEffect != null)
        {
            bool animationFinished = false;
            void OnSpecialEffectAnimationCompleted()
            {
                animationFinished = true;
                AnimationManager.Instance.SpecialEffectAnimationCompleted -= OnSpecialEffectAnimationCompleted;
            }

            AnimationManager.Instance.SpecialEffectAnimationCompleted += OnSpecialEffectAnimationCompleted;
            AnimationManager.Instance.InvokeSpecialEffectAnimationStarted(true);
            yield return new WaitUntil(() => animationFinished);
        }
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
        NewTurnStarted?.Invoke(turnNumber);
        var ctx = new StatusEffectContext(player, currentEnemy, isPlayerEffect: true);

        if (player.StatusEffects.ConsumeSkipTurn())
            yield break;

        player.StatusEffects.TriggerEffects(StatusEffectTrigger.StartOfTurn, ctx);
        player.RegenerateHealth();
        playerUI.UpdateTexts();

        diceManager.ResetTurnRollState();
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

        player.StatusEffects.RemoveEffectsThatExpireAtTurnEnd(true);
        player.StatusEffects.TriggerEffects(StatusEffectTrigger.EndOfTurn, ctx);
        playerUI.UpdateTexts();
        PlayerTurnEnded?.Invoke();
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
        yield return StartCoroutine(EnemyPhysicalAttacks());

        // Magical attacks - one per hit, each with a flash and a delay.
        yield return StartCoroutine(EnemyMagicAttacks());
        
        currentEnemy.Heal(currentEnemy.EnemyStats.healAmount);
        // trigger heal special effects
        currentEnemy.AddShield(currentEnemy.EnemyStats.shieldAmount);
        var eotContext = new SpecialEffectContext(currentEnemy, player, turnNumber, damageAttempted: 0, damageTaken: 0, isMagic: false);
        currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.EndOfTurn, eotContext);
        enemyUI.UpdateTexts();
        currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.AfterNTurns, eotContext);
        enemyUI.UpdateTexts();

        currentEnemy.StatusEffects.RemoveEffectsThatExpireAtTurnEnd(false);
        currentEnemy.StatusEffects.TriggerEffects(StatusEffectTrigger.EndOfTurn, ctx);
        currentEnemy.IncreaseTurnStats(currentStage); // Increase enemy stats based on turn increases

        enemyUI.UpdateTexts();

        yield return null;
    }
    private IEnumerator EnemyPhysicalAttacks()
    {
        for (int i = 0; i < currentEnemy.EnemyStats.physicalAttackAmount; i++)
        {
            EnemyPhysicalAttack?.Invoke();
            int damage = currentEnemy.EnemyStats.physicalAttackDamage;
            var dcontext = new DamageContext(damage, false, currentEnemy.CurrentShield > 0, currentEnemy, player, index: i);
            currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.DealingDamage, null, dcontext);
            int damageTaken = player.TakeDamage(damage + dcontext.Amount, false);
            var pcontext = new SpecialEffectContext(currentEnemy, player, turnNumber, damageAttempted: damage, damageTaken: damageTaken, isMagic: false);
            currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.OnDamageDealt, pcontext, dcontext);
            enemyUI.FlashPhysicalDamageText();
            PlayerDamageTaken?.Invoke(damage);

            playerUI.UpdateTexts();
            enemyUI.UpdateTexts();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator EnemyMagicAttacks()
    {
        for (int i = 0; i < currentEnemy.EnemyStats.magicalAttackAmount; i++)
        {
            EnemyMagicalAttack?.Invoke();
            int damage = currentEnemy.EnemyStats.magicalAttackDamage;
            var dcontext = new DamageContext(damage, false, currentEnemy.CurrentShield > 0, currentEnemy, player, index: i);
            currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.DealingDamage, null, dcontext);
            int damageTaken = player.TakeDamage(damage + dcontext.Amount, true);
            
            var mcontext = new SpecialEffectContext(currentEnemy, player, turnNumber, damageAttempted: damage, damageTaken: damageTaken, isMagic: true);
            currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.OnDamageDealt, mcontext, dcontext);
            enemyUI.FlashMagicalDamageText();
            PlayerDamageTaken?.Invoke(damageTaken);
            playerUI.UpdateTexts();
            enemyUI.UpdateTexts();
            yield return new WaitForSeconds(1f);
        }
    }
    private void OnPlayerWon()
    {
        enemiesDefeated++;
        battleActive = false;
        player.AddCoins(currentEnemy.CoinReward);
        BattleWon?.Invoke();
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
        RunEnded?.Invoke();
        EndBattle();
    }

    public void EndBattle()
    {
        var ctx = new SpecialEffectContext(currentEnemy, player, turnNumber, damageAttempted: 0, damageTaken: 0, isMagic: false);
        currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.EndOfBattle, ctx);
        diceManager.diceManagerUI.ClearDiceUI();
        ShopManager.Instance.OpenShop();
    }

    public void IncreaseStage()
    {
        currentStage++;
        StageIncreased?.Invoke(currentStage);
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

    public void ResumeBattleFromSave()
    {
        if (currentEnemy == null)
        {
            currentEnemy = DetermineEnemy();
            if (currentEnemy != null)
            {
                currentEnemy.ModifyStats(currentStage);
                currentEnemy.InitForBattle();
            }
        }

        battleActive = true;
        enemyUI?.SetEnemy(currentEnemy);
        playerUI?.UpdateTexts();
        cardManagerUI?.RefreshUI();
        StartCoroutine(BattleLoop());
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

    public void NotifyEnemyDamageDealt(int amount, bool isMagic)
    {
        EnemyDamageDealt?.Invoke(amount, isMagic);
        Debug.Log($"[CombatManager] Enemy damage dealt: {amount} (Magic: {isMagic})");
    }

    public void NotifyPlayerShieldGained(int amount)
        => PlayerShieldGained?.Invoke(amount);

    public void NotifyPlayerHealingReceived(int amount)
        => PlayerHealingReceived?.Invoke(amount);

    public void NotifyPlayerCoinsGained(int amount)
        => PlayerCoinsGained?.Invoke(amount);

    
    // UI functions for damage/healing/shield changes. These are separate from the actual damage/healing methods to allow for UI animations to play without affecting the underlying game logic.
    public void NotifyPlayerHealthDamageTakenUI(int amount)
        => PlayerHealthDamageTaken?.Invoke(amount);
    public void NotifyPlayerHealthHealedUI(int amount)
        => PlayerHealthHealed?.Invoke(amount);
    public void NotifyPlayerShieldDamageTakenUI(int amount)
        => PlayerShieldDamageTaken?.Invoke(amount);
    public void NotifyPlayerShieldHealedUI(int amount)
        => PlayerShieldHealed?.Invoke(amount);
    public void NotifyEnemyHealthDamageTakenUI(int amount)
        => EnemyHealthDamageTaken?.Invoke(amount);
    public void NotifyEnemyHealthHealedUI(int amount)
        => EnemyHealthHealed?.Invoke(amount);
    public void NotifyEnemyShieldDamageTakenUI(int amount)
        => EnemyShieldDamageTaken?.Invoke(amount);
    public void NotifyEnemyShieldHealedUI(int amount)
        => EnemyShieldHealed?.Invoke(amount);
    public void NotifyPlayerCoinsChangedUI(int amount)
        => PlayerCoinsChanged?.Invoke(amount);
    public int GetCurrentStage() => currentStage;
    public int GetTurnNumber() => turnNumber;
    public Enemy GetCurrentEnemy() => currentEnemy;
    public void SetCurrentEnemy(Enemy enemy)
    {
        currentEnemy = enemy;
        enemyUI?.SetEnemy(currentEnemy);
    }
    public void SetTurnNumber(int turn)
    {
        turnNumber = turn;
    }
    public void SetCurrentStage(int stage)
    {
        currentStage = stage;
    }
}

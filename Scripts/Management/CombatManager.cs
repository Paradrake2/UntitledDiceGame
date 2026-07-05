using System.Collections;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private Enemy currentEnemy;
    [SerializeField] private EnemyUI enemyUI;
    [SerializeField] private int currentStage = 1;
    [SerializeField] private Player player;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private DiceManager diceManager;
    [SerializeField] private CardManager cardManager;

    private int turnNumber;
    private bool battleActive;
    private int enemiesDefeated;

    [SerializeField] private int gemsPerEnemy = 5;

    public void StartBattle(int stage)
    {
        currentStage = stage;
        enemiesDefeated = 0;
        currentEnemy = DetermineEnemy();
        currentEnemy.ModifyStats(currentStage);
        currentEnemy.InitForBattle();
        player.InitForBattle();

        turnNumber = 0;
        battleActive = true;

        enemyUI.SetEnemy(currentEnemy);
        currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.StartOfBattle, turnNumber, player);

        StartCoroutine(BattleLoop());
    }

    private IEnumerator BattleLoop()
    {
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

        // Play cards in the order the dice appear left-to-right.
        // Each die value maps directly to a card position (1–6).
        int[] values = diceManager.GetValues();
        foreach (int value in values)
        {
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

        var ctx = new StatusEffectContext(player, currentEnemy, isPlayerEffect: false);

        if (currentEnemy.StatusEffects.ConsumeSkipTurn())
        {
            yield return null;
            yield break;
        }

        currentEnemy.StatusEffects.TriggerEffects(StatusEffectTrigger.StartOfTurn, ctx);
        enemyUI.UpdateTexts();

        currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.StartOfTurn, turnNumber, player);
        enemyUI.UpdateTexts();

        // Physical attacks — one per hit, each with a flash and a delay.
        for (int i = 0; i < currentEnemy.EnemyStats.physicalAttackAmount; i++)
        {
            enemyUI.FlashPhysicalDamageText();
            player.TakeDamage(currentEnemy.EnemyStats.physicalAttackDamage, false);
            playerUI.UpdateTexts();
            yield return new WaitForSeconds(1f);
        }

        // Magical attacks — one per hit, each with a flash and a delay.
        for (int i = 0; i < currentEnemy.EnemyStats.magicalAttackAmount; i++)
        {
            enemyUI.FlashMagicalDamageText();
            player.TakeDamage(currentEnemy.EnemyStats.magicalAttackDamage, true);
            playerUI.UpdateTexts();
            yield return new WaitForSeconds(1f);
        }

        currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.EndOfTurn, turnNumber, player);
        enemyUI.UpdateTexts();
        currentEnemy.TriggerSpecialEffect(SpecialEffectTrigger.AfterNTurns, turnNumber, player);
        enemyUI.UpdateTexts();

        currentEnemy.StatusEffects.TriggerEffects(StatusEffectTrigger.EndOfTurn, ctx);
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
        // display results
        // open shop
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
        return currentEnemy; // placeholder
    }
    public void Start()
    {
        StartBattle(currentStage);
    }
}

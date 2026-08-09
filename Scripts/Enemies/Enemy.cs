using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    public int maxHealth;
    public int shield; // amount of shield gained at start of battle
    public int physicalAttackDamage;
    public int physicalAttackAmount;
    public int magicalAttackDamage;
    public int magicalAttackAmount;
    public int healAmount;
    public int shieldAmount; // amount of shield gained after turn, if any
    public bool HasShield => shield > 0;
}

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject
{
    [SerializeField] private string enemyName;
    [SerializeField] private Sprite enemySprite;
    [SerializeField] private int baseMaxHealth;
    [SerializeField] private int baseShield; // amount of shield gained at start of battle
    [SerializeField] private int basePhysicalAttackDamage;
    [SerializeField] private int baseMagicalAttackDamage;
    [SerializeField] private int basePhysicalAttackAmount;
    [SerializeField] private int baseMagicalAttackAmount;
    [SerializeField] private int baseHealAmount; // amount healed every turn, if any
    [SerializeField] private int baseShieldAmount; // amount of shield gained every turn
    [SerializeField] private int coinReward = 10;
    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] private SpecialEffect specialEffect;
    [SerializeField] private int tier;
    [SerializeField] private int shieldTurnIncrease = 0; // How much shield increases per turn, if any.
    [SerializeField] private int healTurnIncrease = 0; // How much healing increases per turn, if any.
    [SerializeField] private int physicalAttackTurnIncrease = 0; // How much physical attack increases per turn, if any.
    [SerializeField] private int magicalAttackTurnIncrease = 0; // How much magical attack increases per turn, if any.

    // Runtime state — reset each battle via InitForBattle()
    private int currentHealth;
    private int currentShield;
    [SerializeField] private StatusEffectHandler statusEffects = new StatusEffectHandler();

    public StatusEffectHandler StatusEffects => statusEffects;

    public int CurrentHealth => currentHealth;
    public int CurrentShield => currentShield;
    public int CoinReward => coinReward;
    public bool IsAlive => currentHealth > 0;
    public string EnemyName => enemyName;
    public Sprite Icon => enemySprite;
    public EnemyStats EnemyStats => enemyStats;
    public SpecialEffect SpecialEffect => specialEffect;
    public int Tier => tier;

    /// <summary>Call after ModifyStats to set runtime health/shield for a new battle.</summary>
    public void InitForBattle()
    {
        currentHealth = enemyStats.maxHealth;
        currentShield = enemyStats.shield;
        enemyStats.physicalAttackAmount = basePhysicalAttackAmount;
        enemyStats.magicalAttackAmount = baseMagicalAttackAmount;
        enemyStats.healAmount = baseHealAmount;
        enemyStats.shieldAmount = baseShieldAmount;
        statusEffects.Clear();
        specialEffect?.ResetRuntimeState();
    }

    /// <summary>Physical damage hits shield first; magic damage bypasses shield entirely.</summary>
    public void TakeDamage(int amount, bool isMagic, float modifier = 1f, int? index = 0)
    {
        amount = Mathf.RoundToInt(amount * modifier);

        var ctx = new StatusEffectContext(FindAnyObjectByType<Player>(), this, isPlayerEffect: false);
        amount = statusEffects.ModifyIncomingDamage(amount, isMagic, ctx);
        DamageContext context = new DamageContext(amount, isMagic, currentShield > 0, this, FindAnyObjectByType<Player>(), index); // Turn number is not relevant here
        if (specialEffect != null)
        {
            specialEffect.ModifyIncomingDamage(context);
            if (specialEffect.TryNegateIncomingDamage(context))
            {
                context.Amount = 0;
                // show ward activated effect, probably triggered via event
            }
        }
        amount = context.Amount;

        Debug.Log("Final damage after status effects and special effects: " + amount);
        if (isMagic)
        {
            currentHealth = Mathf.Max(0, currentHealth - amount);
            CombatManager.Instance?.NotifyEnemyHealthDamageTakenUI(amount); // for UI animation
        }
        else
        {
            int shieldAbsorbed = Mathf.Min(currentShield, amount);
            currentShield -= shieldAbsorbed;
            if (shieldAbsorbed > 0)
            {
                CombatManager.Instance?.NotifyEnemyShieldDamageTakenUI(shieldAbsorbed); // for UI animation
            }
            int remaining = amount - shieldAbsorbed;
            currentHealth = Mathf.Max(0, currentHealth - remaining);
            if (remaining > 0) CombatManager.Instance?.NotifyEnemyHealthDamageTakenUI(amount); // for UI animation

        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void IncreaseTurnStats(int currentStage)
    {
        enemyStats.shield += StageBasedStatIncrease(currentStage, shieldTurnIncrease);
        enemyStats.healAmount += StageBasedStatIncrease(currentStage, healTurnIncrease);
        enemyStats.physicalAttackDamage += StageBasedStatIncrease(currentStage, physicalAttackTurnIncrease);
        enemyStats.magicalAttackDamage += StageBasedStatIncrease(currentStage, magicalAttackTurnIncrease);
    }
    private int StageBasedStatIncrease(int currentStage, int baseAmount)
    {
        float increasePerStage = 0.15f;
        int newAmount = Mathf.RoundToInt(baseAmount * (1 + increasePerStage * currentStage));
        return newAmount;
    }
    public void Die()
    {
        Debug.Log($"{enemyName} has been defeated!");
    }
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(enemyStats.maxHealth, currentHealth + amount);
        if (amount > 0)CombatManager.Instance?.NotifyEnemyHealthHealedUI(amount); // for UI animation
        Debug.Log("" + enemyName + " healed for " + amount + " health. Current health: " + currentHealth);
    }

    public void AddShield(int amount)
    {
        currentShield += amount;
        if (amount > 0) CombatManager.Instance?.NotifyEnemyShieldHealedUI(amount); // for UI animation
        Debug.Log("" + enemyName + " gained " + amount + " shield. Current shield: " + currentShield);
    }

    /// <summary>Fires the special effect if its trigger condition is met.</summary>
    public void TriggerSpecialEffect(SpecialEffectTrigger trigger, SpecialEffectContext ctx)
    {
        if (specialEffect == null) return;
        if (specialEffect.ShouldTrigger(trigger, ctx.turnNumber))
            specialEffect.ApplyEffect(ctx);
    }

    public void ModifyStats(int stage)
    {
        enemyStats.maxHealth = baseMaxHealth + stage * 25;
        enemyStats.shield = baseShield + stage * 10;
        enemyStats.physicalAttackDamage = basePhysicalAttackDamage + stage * 5;
        enemyStats.magicalAttackDamage = baseMagicalAttackDamage + stage * 5;
        enemyStats.healAmount = baseHealAmount + stage * 5;
        enemyStats.shieldAmount = baseShieldAmount + stage * 5;
    }
    public int GetMaxHealth()
    {
        return enemyStats.maxHealth;
    }
    public void SetCurrentHealth(int health)
    {
        currentHealth = Mathf.Clamp(health, 0, enemyStats.maxHealth);
    }
    public void SetCurrentShield(int shield)
    {
        currentShield = Mathf.Max(0, shield);
    }
}

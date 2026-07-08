using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    public int maxHealth;
    public int shield;
    public int physicalAttackDamage;
    public int physicalAttackAmount;
    public int magicalAttackDamage;
    public int magicalAttackAmount;
}

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject
{
    [SerializeField] private string enemyName;
    [SerializeField] private Sprite enemySprite;
    [SerializeField] private int baseMaxHealth;
    [SerializeField] private int baseShield;
    [SerializeField] private int basePhysicalAttackDamage;
    [SerializeField] private int baseMagicalAttackDamage;
    [SerializeField] private int basePhysicalAttackAmount;
    [SerializeField] private int baseMagicalAttackAmount;
    [SerializeField] private int coinReward = 10;
    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] private SpecialEffect specialEffect;

    // Runtime state — reset each battle via InitForBattle()
    private int currentHealth;
    private int currentShield;
    private StatusEffectHandler statusEffects = new StatusEffectHandler();

    public StatusEffectHandler StatusEffects => statusEffects;

    public int CurrentHealth => currentHealth;
    public int CurrentShield => currentShield;
    public int CoinReward => coinReward;
    public bool IsAlive => currentHealth > 0;
    public string EnemyName => enemyName;
    public Sprite Icon => enemySprite;
    public EnemyStats EnemyStats => enemyStats;

    /// <summary>Call after ModifyStats to set runtime health/shield for a new battle.</summary>
    public void InitForBattle()
    {
        currentHealth = enemyStats.maxHealth;
        currentShield = enemyStats.shield;
        statusEffects.Clear();
    }

    /// <summary>Physical damage hits shield first; magic damage bypasses shield entirely.</summary>
    public void TakeDamage(int amount, bool isMagic, float modifier = 1f)
    {
        amount = Mathf.RoundToInt(amount * modifier);

        DamageContext context = new DamageContext(amount, isMagic, currentShield > 0, this, null);
        if (specialEffect != null) specialEffect.ModifyIncomingDamage(context);
        amount = context.Amount;
        if (isMagic)
        {
            currentHealth = Mathf.Max(0, currentHealth - amount);
        }
        else
        {
            int shieldAbsorbed = Mathf.Min(currentShield, amount);
            currentShield -= shieldAbsorbed;
            int remaining = amount - shieldAbsorbed;
            currentHealth = Mathf.Max(0, currentHealth - remaining);
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Debug.Log($"{enemyName} has been defeated!");
    }
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(enemyStats.maxHealth, currentHealth + amount);
    }

    public void AddShield(int amount)
    {
        currentShield += amount;
    }

    /// <summary>Deals all physical and magical attacks to the player.</summary>
    public void Attack(Player player)
    {
        for (int i = 0; i < enemyStats.physicalAttackAmount; i++)
            player.TakeDamage(enemyStats.physicalAttackDamage, false);

        for (int i = 0; i < enemyStats.magicalAttackAmount; i++)
            player.TakeDamage(enemyStats.magicalAttackDamage, true);
    }

    /// <summary>Fires the special effect if its trigger condition is met.</summary>
    public void TriggerSpecialEffect(SpecialEffectTrigger trigger, int turnNumber, Player player)
    {
        if (specialEffect == null) return;
        if (specialEffect.ShouldTrigger(trigger, turnNumber))
            specialEffect.ApplyEffect(this, player);
    }

    public void ModifyStats(int stage)
    {
        enemyStats.maxHealth = baseMaxHealth + stage * 25;
        enemyStats.shield = baseShield + stage * 10;
        enemyStats.physicalAttackDamage = basePhysicalAttackDamage + stage * 5;
        enemyStats.magicalAttackDamage = baseMagicalAttackDamage + stage * 5;
    }
}

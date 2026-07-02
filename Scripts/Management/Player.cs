using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private CardManager cardManager;

    private int currentHealth;
    private int currentShield;
    private int coins;
    private StatusEffectHandler statusEffects = new StatusEffectHandler();

    // Upgrade-driven stats — refreshed at the start of each battle via InitForBattle().
    private int outgoingDamageBonus;
    private int damageReduction;
    private int coinBonus;
    private int revivesRemaining;

    public StatusEffectHandler StatusEffects => statusEffects;
    public int OutgoingDamageBonus => outgoingDamageBonus;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public int CurrentShield => currentShield;
    public int Coins => coins;
    public int RevivesRemaining => revivesRemaining;
    public bool IsAlive => currentHealth > 0;
    public CardManager CardManager => cardManager;

    public void InitForBattle()
    {
        currentHealth = maxHealth;
        currentShield = 0;
        statusEffects.Clear();

        if (UpgradeManager.Instance != null)
        {
            outgoingDamageBonus = UpgradeManager.Instance.GetTotalInt(UpgradeType.OutgoingDamageBonus);
            damageReduction     = UpgradeManager.Instance.GetTotalInt(UpgradeType.DamageReduction);
            coinBonus           = UpgradeManager.Instance.GetTotalInt(UpgradeType.CoinBonus);
            revivesRemaining    = UpgradeManager.Instance.GetTotalInt(UpgradeType.Revive);
        }
    }

    /// <summary>
    /// Physical damage hits shield first; magic damage bypasses shield entirely.
    /// Damage reduction from upgrades is applied before shield calculations.
    /// </summary>
    public void TakeDamage(int amount, bool isMagic)
    {
        int reduced = Mathf.Max(0, amount - damageReduction);
        if (isMagic)
        {
            currentHealth = Mathf.Max(0, currentHealth - reduced);
        }
        else
        {
            int shieldAbsorbed = Mathf.Min(currentShield, reduced);
            currentShield -= shieldAbsorbed;
            int remaining = reduced - shieldAbsorbed;
            currentHealth = Mathf.Max(0, currentHealth - remaining);
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }

    public void AddShield(int amount)
    {
        currentShield += amount;
    }

    public void AddCoins(int amount)
    {
        coins += amount + coinBonus;
    }

    /// <summary>
    /// Consumes one revive charge, restoring the player to full health.
    /// Returns true if a revive was available.
    /// </summary>
    public bool TryRevive()
    {
        if (revivesRemaining <= 0) return false;
        revivesRemaining--;
        currentHealth = maxHealth;
        currentShield = 0;
        statusEffects.Clear();
        return true;
    }
}

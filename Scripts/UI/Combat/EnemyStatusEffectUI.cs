using UnityEngine;

public class EnemyStatusEffectUI : MonoBehaviour
{
    [SerializeField] private GameObject statusEffectPrefab;
    [SerializeField] private CombatManager combatManager;
    [SerializeField] private Enemy enemy;
    void OnEnable()
    {
        // Subscribe to the EnemyStatusEffectChanged event
        combatManager.StatusEffectApplied += UpdateStatusEffectsUI;
        combatManager.StatusEffectRemoved += UpdateStatusEffectsUI;
        combatManager.StatusEffectTriggered += UpdateStatusEffectsUI;
        combatManager.EnemySelected += SetEnemy;
    }
    void OnDisable()
    {
        // Unsubscribe from the EnemyStatusEffectChanged event
        combatManager.StatusEffectApplied -= UpdateStatusEffectsUI;
        combatManager.StatusEffectRemoved -= UpdateStatusEffectsUI;
        combatManager.StatusEffectTriggered -= UpdateStatusEffectsUI;
        combatManager.EnemySelected -= SetEnemy;
    }
    public void SetEnemy(Enemy enemy)
    {
        this.enemy = enemy;
        UpdateStatusEffectsUI(null, false); // Update the UI when the enemy is set
    }
    public void UpdateStatusEffectsUI(StatusEffect effect, bool isPlayer)
    {
        if (isPlayer) return; // Only update for enemy status effects
        // Clear existing status effect UI elements
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        SortStatusEffectsByDuration(enemy);
        // Create new status effect UI elements based on the enemy's active effects
        foreach (var effectInstance in enemy.StatusEffects.ActiveEffects)
        {
            GameObject effectUI = Instantiate(statusEffectPrefab, transform);
            StatusEffectUI effectUIScript = effectUI.GetComponent<StatusEffectUI>();
            if (effectUIScript != null)
            {
                effectUIScript.SetStatusEffect(effectInstance.Effect, effectInstance.RemainingDuration);
            }
        }
    }
    private void SortStatusEffectsByDuration(Enemy enemy)
    {
        // Sort the active effects by remaining duration in descending order
        enemy.StatusEffects.SortEffectsByDuration();
    }
}

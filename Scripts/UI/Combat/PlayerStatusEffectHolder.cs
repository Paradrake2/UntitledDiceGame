using UnityEngine;

public class PlayerStatusEffectHolder : MonoBehaviour
{
    [SerializeField] private GameObject statusEffectPrefab;
    [SerializeField] private CombatManager combatManager;
    [SerializeField] private Player player;
    void OnEnable()
    {
        // Subscribe to the EnemyStatusEffectChanged event
        combatManager.StatusEffectApplied += UpdateStatusEffectsUI;
        combatManager.StatusEffectRemoved += UpdateStatusEffectsUI;
        combatManager.StatusEffectTriggered += UpdateStatusEffectsUI;
    }
    void OnDisable()
    {
        // Unsubscribe from the EnemyStatusEffectChanged event
        combatManager.StatusEffectApplied -= UpdateStatusEffectsUI;
        combatManager.StatusEffectRemoved -= UpdateStatusEffectsUI;
        combatManager.StatusEffectTriggered -= UpdateStatusEffectsUI;
    }
    public void UpdateStatusEffectsUI(StatusEffect effect, bool isPlayer)
    {
        // Clear existing status effect UI elements
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        SortStatusEffectsByDuration(player);
        // Create new status effect UI elements based on the enemy's active effects
        foreach (var effectInstance in player.StatusEffects.ActiveEffects)
        {
            GameObject effectUI = Instantiate(statusEffectPrefab, transform);
            StatusEffectUI effectUIScript = effectUI.GetComponent<StatusEffectUI>();
            if (effectUIScript != null)
            {
                effectUIScript.SetStatusEffect(effectInstance.Effect, effectInstance.RemainingDuration);
            }
        }
    }
    private void SortStatusEffectsByDuration(Player player)
    {
        // Sort the active effects by remaining duration in descending order
        player.StatusEffects.SortEffectsByDuration();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

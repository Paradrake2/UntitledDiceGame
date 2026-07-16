using UnityEngine;

public class DamageManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private static DamageManager _instance;
    public static DamageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<DamageManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("DamageManager");
                    _instance = obj.AddComponent<DamageManager>();
                }
            }
            return _instance;
        }
    }
    public void ApplyDamageToEnemy(Enemy enemy, Player player, int damage, bool isMagic, int? cardIndex = null)
    {
        if (enemy == null)
        {
            Debug.LogError("Enemy is null. Cannot apply damage.");
            return;
        }
        var ctx = new StatusEffectContext(player, enemy, isPlayerEffect: true);
        damage = player.StatusEffects.ModifyOutgoingDamage(damage, isMagic, ctx);
        CombatManager.Instance?.NotifyEnemyDamageDealt(damage, isMagic);
        enemy.TakeDamage(damage, isMagic, 1f, cardIndex);
    }
    public void ApplyDamageToPlayer(Player player, int damage, bool isMagic)
    {
        if (player == null)
        {
            Debug.LogError("Player is null. Cannot apply damage.");
            return;
        }
        var ctx = new StatusEffectContext(player, null, isPlayerEffect: false);
        damage = player.StatusEffects.ModifyIncomingDamage(damage, isMagic, ctx);
        player.TakeDamage(damage, isMagic);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

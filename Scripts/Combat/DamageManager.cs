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
    public void ApplyDamageToEnemy(Enemy enemy, Player player,int damage, bool isMagic)
    {
        if (enemy == null)
        {
            Debug.LogError("Enemy is null. Cannot apply damage.");
            return;
        }
        var ctx = new StatusEffectContext(player, enemy, isPlayerEffect: true);
        damage = player.StatusEffects.ModifyOutgoingDamage(damage, isMagic, ctx);
        enemy.TakeDamage(damage, isMagic);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

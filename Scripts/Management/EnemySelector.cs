using UnityEngine;


public class EnemySelector : MonoBehaviour
{
    private static EnemySelector instance;
    public static EnemySelector Instance => instance;
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private EnemyTierStageEntry[] enemyTierStageEntries;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public Enemy DetermineEnemy(int stage)
    {
        // Choose which enemy to spawn from a pool based on current stage
        foreach (var entry in enemyTierStageEntries)
        {
            if (stage >= entry.MinStage && stage <= entry.MaxStage)
            {
                int tier = entry.EnemyTier;
                EnemyTier enemyTier = System.Array.Find(enemyPool.EnemyTiers, t => t.tier == tier);
                if (enemyTier != null)
                {
                    // Select an enemy from the tier based on weights
                    int totalWeight = 0;
                    foreach (var enemyEntry in enemyTier.Enemies)
                    {
                        totalWeight += enemyEntry.weight;
                    }
                    int randomValue = Random.Range(0, totalWeight);
                    foreach (var enemyEntry in enemyTier.Enemies)
                    {
                        if (randomValue < enemyEntry.weight)
                        {
                            Enemy enemyToSpawn = Instantiate(enemyEntry.enemy);
                            return enemyToSpawn;
                        }
                        randomValue -= enemyEntry.weight;
                    }
                }
            }
        }
        return null; // placeholder
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

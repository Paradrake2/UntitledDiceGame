using UnityEngine;

[System.Serializable]
public class EnemyPoolEntry
{
    public Enemy enemy;
    public int weight;
}

[System.Serializable]
public class EnemyTier
{
    public int tier;
    [SerializeField] private EnemyPoolEntry[] enemies;
    public EnemyPoolEntry[] Enemies => enemies;
}
[CreateAssetMenu(fileName = "EnemyPool", menuName = "EnemyPool/EnemyPool")]
public class EnemyPool : ScriptableObject
{
    [SerializeField] private EnemyTier[] enemyTiers;
    public EnemyTier[] EnemyTiers => enemyTiers;
}

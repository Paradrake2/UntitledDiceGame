using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTierStageEntry", menuName = "Scriptable Objects/EnemyTierStageEntry")]
public class EnemyTierStageEntry : ScriptableObject
{
    [SerializeField] private int minStage;
    [SerializeField] private int maxStage;
    [SerializeField] private int enemyTier;
    public int MinStage => minStage;
    public int MaxStage => maxStage;
    public int EnemyTier => enemyTier;
}

using UnityEngine;

/// <summary>
/// Persistent singleton that stores all-time achievement statistics in PlayerPrefs.
/// </summary>
public class GameStatistics : MonoBehaviour
{
    public static GameStatistics Instance { get; private set; }

    private const string GamesPlayedKey     = "AchStat_GamesPlayed";
    private const string EnemiesDefeatedKey = "AchStat_EnemiesDefeated";

    public int TotalGamesPlayed     { get; private set; }
    public int TotalEnemiesDefeated { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }

    public void RecordGamePlayed()
    {
        TotalGamesPlayed++;
        PlayerPrefs.SetInt(GamesPlayedKey, TotalGamesPlayed);
        PlayerPrefs.Save();
    }

    public void RecordEnemyDefeated()
    {
        TotalEnemiesDefeated++;
        PlayerPrefs.SetInt(EnemiesDefeatedKey, TotalEnemiesDefeated);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        TotalGamesPlayed     = PlayerPrefs.GetInt(GamesPlayedKey,     0);
        TotalEnemiesDefeated = PlayerPrefs.GetInt(EnemiesDefeatedKey, 0);
    }
}

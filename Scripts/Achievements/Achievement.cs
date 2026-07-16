using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "Achievements/Achievement")]
public class Achievement : ScriptableObject
{
    [SerializeField] private string achievementId;
    [SerializeField] private string displayName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private AchievementCondition condition;
    [SerializeField] private Card cardToUnlock;
    [SerializeField] private bool isUnlocked;

    public string AchievementId => achievementId;
    public string DisplayName => displayName;
    public string Description => description;
    public Sprite Icon => icon;
    public AchievementCondition Condition => condition;
    public Card CardToUnlock => cardToUnlock;
    public bool IsUnlocked => isUnlocked;
    public void SetUnlocked(bool unlocked)
    {
        isUnlocked = unlocked;
    }
}

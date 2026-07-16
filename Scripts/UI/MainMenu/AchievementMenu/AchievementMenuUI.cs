using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject achievementPrefab;
    [SerializeField] private Transform achievementListParent;
    [SerializeField] private AchievementManager achievementManager;
    [SerializeField] private TextMeshProUGUI achievementDescriptionText;
    public void LoadAchievements()
    {
        FindAchievementManager();
        foreach (Transform child in achievementListParent)
        {
            Destroy(child.gameObject);
        }
        foreach (var achievement in achievementManager.Achievements)
        {
            GameObject achievementGO = Instantiate(achievementPrefab, achievementListParent);
            AchievementSlotUI slotUI = achievementGO.GetComponent<AchievementSlotUI>();
            if (slotUI != null)
            {
                slotUI.SetAchievement(achievement, this);
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadAchievements();
    }
    private void FindAchievementManager()
    {
        if (achievementManager == null)
        {
            achievementManager = FindAnyObjectByType<AchievementManager>();
        }
    }
    public void ShowAchievementDescription(Achievement achievement)
    {
        if (achievement != null)
        {
            achievementDescriptionText.text = achievement.DisplayName + "\n" + achievement.Description;
        }
        else
        {
            achievementDescriptionText.text = "";
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

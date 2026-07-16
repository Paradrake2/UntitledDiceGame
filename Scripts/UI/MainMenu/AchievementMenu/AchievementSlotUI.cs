using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class AchievementSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Achievement achievement;
    [SerializeField] private AchievementMenuUI achievementMenuUI;
    public void SetAchievement(Achievement achievement, AchievementMenuUI achievementMenuUI)
    {
        this.achievement = achievement;
        iconImage.sprite = achievement.Icon;
        this.achievementMenuUI = achievementMenuUI;
    }
    private void IsUnlocked()
    {
        if (achievement.IsUnlocked)
        {
            // Show unlocked state (e.g., change icon color, show a checkmark, etc.)
            iconImage.color = Color.white; // Example: set to normal color
        }
        else
        {
            // Show locked state (e.g., grayscale the icon, show a lock icon, etc.)
            iconImage.color = Color.gray; // Example: set to gray
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // show tooltip with achievement description
        achievementMenuUI.ShowAchievementDescription(achievement);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // hide tooltip
        achievementMenuUI.ShowAchievementDescription(null);
    }
}

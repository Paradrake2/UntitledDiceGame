using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    
    [SerializeField] private GameObject StartGameButton;
    [SerializeField] private GameObject ContinueGameButton;
    [SerializeField] private GameObject QuitGameButton;
    [SerializeField] private Transform upgradePanel;
    [SerializeField] private Transform achievementPanel;
    public void StartGame()
    {
        // select starting cards
        // start game, go to game scene
        SceneManager.LoadScene("Game");
    }
    public void ContinueGame()
    {
        
    }
    public void OpenUpgradePanel()
    {
        
    }
    public void OpenAchievementPanel()
    {
        
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

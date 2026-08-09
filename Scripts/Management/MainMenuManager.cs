using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }

    [SerializeField] private GameObject StartGameButton;
    [SerializeField] private GameObject ContinueGameButton;
    [SerializeField] private GameObject QuitGameButton;
    [SerializeField] private Transform upgradePanel;
    [SerializeField] private Transform achievementPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void StartGame()
    {
        InProgressGame.ClearSave();
        SceneManager.LoadScene("Game");
    }

    public void ContinueGame()
    {
        if (!InProgressGame.HasSave())
        {
            return;
        }

        SceneManager.LoadScene("Game");
    }

    public void OpenUpgradePanel()
    {

    }

    public void OpenAchievementPanel()
    {

    }

    private void Start()
    {
        RefreshContinueButton();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshContinueButton();

        if (scene.name == "Game" && InProgressGame.HasSave())
        {
            InProgressGame loadedGame = InProgressGame.LoadFromPlayerPrefs();
            if (loadedGame != null)
            {
                loadedGame.LoadGame();
                CombatManager.Instance?.ResumeBattleFromSave();
            }
        }
    }

    private void RefreshContinueButton()
    {
        if (ContinueGameButton != null)
        {
            ContinueGameButton.SetActive(InProgressGame.HasSave());
        }
    }

    private void OnApplicationQuit()
    {
        if (CombatManager.Instance != null)
        {
            InProgressGame.CreateFromCurrentState().SaveToPlayerPrefs();
        }
    }
}

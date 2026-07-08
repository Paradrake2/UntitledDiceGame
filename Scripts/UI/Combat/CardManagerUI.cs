using UnityEngine;

public class CardManagerUI : MonoBehaviour
{
    [SerializeField] private BattleCardManager cardManager;
    
    void OnEnable()
    {
        CombatManager.Instance.BattleStarted += RefreshHandler;
    }

    void OnDisable()
    {
        CombatManager.Instance.BattleStarted -= RefreshHandler;
    }

    public void UpdateCardUI(int position, Card newCard)
    {
        if (cardManager == null) 
        {
            Debug.LogError("BattleCardManager is not assigned to CardManagerUI");
            return;
        }

        CardUI cardUI = cardManager.GetCardUI(position);
        
        if (cardUI != null)
        {
            Debug.Log($"Updating card UI at position {position} with new card: {newCard?.name ?? "null"}");
            cardUI.SetCard(newCard);
        }
        else
        {
            Debug.LogError($"CardUI at position {position} is not assigned in BattleCardManager.");
        }
    }
    public void RefreshHandler()
    {
        Debug.Log("Battle started. Refreshing UI...");
        RefreshUI();
    }
    public void RefreshUI()
    {
        Debug.Log("Refreshing CardManagerUI...");
        UpdateCardUI(1, cardManager.GetCard(1));
        UpdateCardUI(2, cardManager.GetCard(2));
        UpdateCardUI(3, cardManager.GetCard(3));
        UpdateCardUI(4, cardManager.GetCard(4));
        UpdateCardUI(5, cardManager.GetCard(5));
        UpdateCardUI(6, cardManager.GetCard(6));
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefreshUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

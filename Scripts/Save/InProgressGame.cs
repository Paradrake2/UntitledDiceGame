using System;
using UnityEngine;

// Stores the data for a game that is currently in progress so it can be saved and loaded later.
[System.Serializable]
public class InProgressGame
{
    private const string SaveKey = "InProgressGame";

    public int currentCoins;
    public int currentStage;
    public string[] ownedCardNames;
    public string index1;
    public string index2;
    public string index3;
    public string index4;
    public string index5;
    public string index6;
    public string currentEnemyName;
    public int currentTurn;
    public int currentEnemyHealth;
    public int currentEnemyShield;
    public int currentPlayerHealth;
    public int currentPlayerShield;
    public int currentRevivesRemaining;

    public InProgressGame(int currentCoins, int currentStage, string[] ownedCardNames, string index1, string index2, string index3, string index4, string index5, string index6, string currentEnemyName, int currentTurn, int currentEnemyHealth, int currentEnemyShield, int currentPlayerHealth, int currentPlayerShield, int currentRevivesRemaining)
    {
        this.currentCoins = currentCoins;
        this.currentStage = currentStage;
        this.ownedCardNames = ownedCardNames;
        this.index1 = index1;
        this.index2 = index2;
        this.index3 = index3;
        this.index4 = index4;
        this.index5 = index5;
        this.index6 = index6;
        this.currentEnemyName = currentEnemyName;
        this.currentTurn = currentTurn;
        this.currentEnemyHealth = currentEnemyHealth;
        this.currentEnemyShield = currentEnemyShield;
        this.currentPlayerHealth = currentPlayerHealth;
        this.currentPlayerShield = currentPlayerShield;
        this.currentRevivesRemaining = currentRevivesRemaining;
    }

    public static InProgressGame CreateFromCurrentState()
    {
        Player player = GameObject.FindFirstObjectByType<Player>();
        CombatManager combatManager = CombatManager.Instance;
        BattleCardManager battleCardManager = BattleCardManager.Instance;

        int coins = player != null ? player.Coins : 0;
        int stage = combatManager != null ? combatManager.GetCurrentStage() : 1;

        Card[] runCards = battleCardManager != null ? battleCardManager.GetRunCards() : new Card[0];
        string[] ownedCardNames = new string[runCards.Length];
        for (int i = 0; i < runCards.Length; i++)
        {
            ownedCardNames[i] = runCards[i] != null ? runCards[i].name : string.Empty;
        }

        string currentEnemyName = combatManager != null && combatManager.GetCurrentEnemy() != null ? combatManager.GetCurrentEnemy().name : string.Empty;
        int turn = combatManager != null ? combatManager.GetTurnNumber() : 0;
        int enemyHealth = combatManager != null && combatManager.GetCurrentEnemy() != null ? combatManager.GetCurrentEnemy().CurrentHealth : 0;
        int enemyShield = combatManager != null && combatManager.GetCurrentEnemy() != null ? combatManager.GetCurrentEnemy().CurrentShield : 0;
        int playerHealth = player != null ? player.CurrentHealth : 0;
        int playerShield = player != null ? player.CurrentShield : 0;
        int revivesRemaining = player != null ? player.RevivesRemaining : 0;

        return new InProgressGame(
            coins,
            stage,
            ownedCardNames,
            GetCardName(battleCardManager, 1),
            GetCardName(battleCardManager, 2),
            GetCardName(battleCardManager, 3),
            GetCardName(battleCardManager, 4),
            GetCardName(battleCardManager, 5),
            GetCardName(battleCardManager, 6),
            currentEnemyName,
            turn,
            enemyHealth,
            enemyShield,
            playerHealth,
            playerShield,
            revivesRemaining);
    }

    public void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(this));
        PlayerPrefs.Save();
    }

    public static InProgressGame LoadFromPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
        {
            return null;
        }

        string json = PlayerPrefs.GetString(SaveKey);
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }

        return JsonUtility.FromJson<InProgressGame>(json);
    }

    public static bool HasSave()
    {
        return PlayerPrefs.HasKey(SaveKey) && !string.IsNullOrEmpty(PlayerPrefs.GetString(SaveKey));
    }

    public static void ClearSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        Player player = GameObject.FindFirstObjectByType<Player>();
        if (player != null)
        {
            player.SetCoins(currentCoins);
            player.SetCurrentHealth(currentPlayerHealth);
            player.SetCurrentShield(currentPlayerShield);
            player.SetRevivesRemaining(currentRevivesRemaining);
        }

        if (CombatManager.Instance != null)
        {
            CombatManager.Instance.SetCurrentStage(currentStage);
            CombatManager.Instance.SetTurnNumber(currentTurn);
            CombatManager.Instance.SetCurrentEnemy(ResolveEnemy(currentEnemyName));
            if (CombatManager.Instance.GetCurrentEnemy() != null)
            {
                CombatManager.Instance.GetCurrentEnemy().SetCurrentHealth(currentEnemyHealth);
                CombatManager.Instance.GetCurrentEnemy().SetCurrentShield(currentEnemyShield);
            }
        }

        if (BattleCardManager.Instance != null)
        {
            BattleCardManager.Instance.runCards = ResolveRunCards(ownedCardNames);
            BattleCardManager.Instance.SetCard(1, ResolveCardByName(index1));
            BattleCardManager.Instance.SetCard(2, ResolveCardByName(index2));
            BattleCardManager.Instance.SetCard(3, ResolveCardByName(index3));
            BattleCardManager.Instance.SetCard(4, ResolveCardByName(index4));
            BattleCardManager.Instance.SetCard(5, ResolveCardByName(index5));
            BattleCardManager.Instance.SetCard(6, ResolveCardByName(index6));
        }
    }

    private static string GetCardName(BattleCardManager battleCardManager, int position)
    {
        Card card = battleCardManager != null ? battleCardManager.GetCard(position) : null;
        return card != null ? card.name : string.Empty;
    }

    private static Card ResolveCardByName(string cardName)
    {
        if (string.IsNullOrEmpty(cardName))
        {
            return null;
        }

        if (CardManager.Instance != null)
        {
            if (CardManager.Instance.unlockedCards != null)
            {
                foreach (Card card in CardManager.Instance.unlockedCards)
                {
                    if (card != null && card.name == cardName)
                    {
                        return CardManager.Instance.CreateRuntimeCard(card);
                    }
                }
            }

            if (CardManager.Instance.AllCards != null)
            {
                foreach (Card card in CardManager.Instance.AllCards)
                {
                    if (card != null && card.name == cardName)
                    {
                        return CardManager.Instance.CreateRuntimeCard(card);
                    }
                }
            }

            if (CardManager.Instance.defaultCards != null)
            {
                foreach (Card card in CardManager.Instance.defaultCards)
                {
                    if (card != null && card.name == cardName)
                    {
                        return CardManager.Instance.CreateRuntimeCard(card);
                    }
                }
            }
        }

        foreach (Card card in Resources.FindObjectsOfTypeAll<Card>())
        {
            if (card != null && card.name == cardName)
            {
                return card;
            }
        }

        return null;
    }

    private static Card[] ResolveRunCards(string[] cardNames)
    {
        if (cardNames == null || cardNames.Length == 0)
        {
            return new Card[0];
        }

        Card[] resolvedCards = new Card[cardNames.Length];
        for (int i = 0; i < cardNames.Length; i++)
        {
            resolvedCards[i] = ResolveCardByName(cardNames[i]);
        }

        return resolvedCards;
    }

    private static Enemy ResolveEnemy(string enemyName)
    {
        if (string.IsNullOrEmpty(enemyName))
        {
            return null;
        }

        foreach (Enemy enemy in Resources.FindObjectsOfTypeAll<Enemy>())
        {
            if (enemy != null && enemy.name == enemyName)
            {
                return UnityEngine.Object.Instantiate(enemy);
            }
        }

        return null;
    }
}

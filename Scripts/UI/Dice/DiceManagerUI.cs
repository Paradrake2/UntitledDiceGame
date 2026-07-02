using UnityEngine;

public class DiceManagerUI : MonoBehaviour
{
    [SerializeField] private DiceManager diceManager;
    [SerializeField] private GameObject dicePrefab;
    public void UpdateDiceUI(int[] diceValues)
    {
        // Clear existing dice UI elements
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Create new dice UI elements based on the current dice values
        for (int i = 0; i < diceValues.Length; i++)
        {
            GameObject newDice = Instantiate(dicePrefab, transform);
            Dice diceComponent = newDice.GetComponent<Dice>();
            if (diceComponent != null)
            {
                diceComponent.SetValue(diceValues[i]);
                diceComponent.Initialize(i, diceManager);
            }
            else
            {
                Debug.LogError("Dice component not found on the dice prefab.");
            }
        }
    }
    public void OnDiceFinalized(int[] finalValues)
    {
        UpdateDiceUI(finalValues);
    }
    public void ClearDiceUI()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    void Start()
    {
        if (diceManager != null)
        {
            diceManager.OnDiceFinalized += UpdateDiceUI;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

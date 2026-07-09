using UnityEngine;

public class SecondaryDiceUI : MonoBehaviour
{
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private GameObject secondaryDieContainer; // Container for the secondary die UI
    [SerializeField] private DiceManager diceManager;
    void OnEnable()
    {
        diceManager.OnSecondaryDieRolled += UpdateSecondaryDieUI;
    }
    void OnDisable()
    {
        diceManager.OnSecondaryDieRolled -= UpdateSecondaryDieUI;
    }
    private void UpdateSecondaryDieUI(int dieValue)
    {
        // Clear existing secondary die UI elements
        foreach (Transform child in secondaryDieContainer.transform)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Updating secondary die UI with value: " + dieValue);
        // Update the UI to reflect the new secondary die value
        GameObject newDice = Instantiate(dicePrefab, secondaryDieContainer.transform);
        Dice diceComponent = newDice.GetComponent<Dice>();
        if (diceComponent != null)
        {
            diceComponent.SetValue(dieValue);
            // Intentionally not setting index as there is no index for secondary dice
        }
        else
        {
            Debug.LogError("Dice component not found on the dice prefab.");
        }


        Debug.Log($"Secondary die rolled: {dieValue}");
        // Here you would update your UI elements, e.g., text or images, to show the new die value
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

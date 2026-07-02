using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI shieldText;
    [SerializeField] private TextMeshProUGUI coinsText;
    public void UpdateTexts()
    {
        if (player != null)
        {
            healthText.text = player.CurrentHealth.ToString();
            shieldText.text = player.CurrentShield.ToString();
            coinsText.text = player.Coins.ToString();
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

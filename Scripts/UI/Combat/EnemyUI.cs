using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private Image enemyImage;
    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private TextMeshProUGUI enemyHealthText;
    [SerializeField] private TextMeshProUGUI enemyShieldText;
    [SerializeField] private TextMeshProUGUI physicalDamageText;
    [SerializeField] private TextMeshProUGUI magicalDamageText;
    public void UpdateTexts()
    {
        if (enemy != null)
        {
            enemyNameText.text = enemy.EnemyName;
            enemyHealthText.text = enemy.CurrentHealth.ToString();
            enemyShieldText.text = enemy.CurrentShield.ToString();
            physicalDamageText.text = enemy.EnemyStats.physicalAttackDamage.ToString() + " x" + enemy.EnemyStats.physicalAttackAmount.ToString();
            magicalDamageText.text = enemy.EnemyStats.magicalAttackDamage.ToString() + " x" + enemy.EnemyStats.magicalAttackAmount.ToString();
        }
    }
    public void SetEnemy(Enemy newEnemy)
    {
        enemy = newEnemy;
        if (enemyImage != null && enemy != null)
        {
            enemyImage.sprite = enemy.Icon;
        }
        UpdateTexts();
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

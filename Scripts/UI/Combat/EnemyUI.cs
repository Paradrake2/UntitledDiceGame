using System.Collections;
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

    public void FlashPhysicalDamageText() => StartCoroutine(FlashText(physicalDamageText));
    public void FlashMagicalDamageText()  => StartCoroutine(FlashText(magicalDamageText));

    private IEnumerator FlashText(TextMeshProUGUI text)
    {
        if (text == null) yield break;

        Color originalColor = text.color;
        Color purple = new Color(0.6f, 0f, 1f);
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            text.color = Color.Lerp(originalColor, purple, elapsed / duration);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            text.color = Color.Lerp(purple, originalColor, elapsed / duration);
            yield return null;
        }

        text.color = originalColor;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

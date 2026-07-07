using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    [SerializeField] private Card card;
    [SerializeField] private Image cardImage;
    [SerializeField] private Image cardIcon; // this is the sprite
    [SerializeField] private TextMeshProUGUI line1Text;
    [SerializeField] private TextMeshProUGUI line2Text;
    [SerializeField] private TextMeshProUGUI line3Text;

    public void SetCard(Card newCard)
    {
        card = newCard;
        if (cardIcon != null && card != null)
        {
            cardIcon.sprite = card.CardSprite;
        }
        // Update the UI elements based on the new card's properties
        // For example, you might want to update the card's image, name, description, etc.
        if (line1Text != null && card != null)
        {
            line1Text.text = card.Line1;
            line1Text.color = card.Line1Color; // Set the color for line 1
        }
        if (line2Text != null && card != null)
        {
            line2Text.text = card.Line2;
            line2Text.color = card.Line2Color; // Set the color for line 2
        }
        if (line3Text != null && card != null)
        {
            line3Text.text = card.Line3;
            line3Text.color = card.Line3Color; // Set the color for line 3
        }
    }

    public void PlayFlashAnimation()
    {
        StartCoroutine(FlashPurple());
    }

    private IEnumerator FlashPurple()
    {
        if (cardImage == null) yield break;

        Color originalColor = cardImage.color;
        Color purple = new Color(0.6f, 0f, 1f);
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cardImage.color = Color.Lerp(originalColor, purple, elapsed / duration);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cardImage.color = Color.Lerp(purple, originalColor, elapsed / duration);
            yield return null;
        }

        cardImage.color = originalColor;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

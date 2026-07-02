using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] private Card card;
    [SerializeField] private Image cardImage;

    public void SetCard(Card newCard)
    {
        card = newCard;
        if (cardImage != null && card != null)
        {
            cardImage.sprite = card.CardSprite;
        }
        // Update the UI elements based on the new card's properties
        // For example, you might want to update the card's image, name, description, etc.
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

using UnityEngine;
using TMPro;
using System.Collections;

// This class creates a fresh floating text instance for each change event.
// Subclasses decide what event should trigger the animation.
public abstract class ValueAnimation : MonoBehaviour
{
    [SerializeField] private GameObject valueTextTemplate;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float moveDistance = 50f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] protected CombatManager combatManager;

    public void PlayAnimation(int value, Transform anchor = null)
    {
        if (valueTextTemplate == null)
        {
            Debug.LogWarning($"{nameof(ValueAnimation)}: No value text template assigned.", this);
            return;
        }

        Transform parentTransform = anchor != null ? anchor.parent : transform.parent;
        GameObject instance = Instantiate(valueTextTemplate, parentTransform, false);
        instance.SetActive(true);

        TextMeshProUGUI textComponent = instance.GetComponent<TextMeshProUGUI>();
        if (textComponent == null)
        {
            Debug.LogWarning($"{nameof(ValueAnimation)}: The assigned template does not have a TextMeshProUGUI component.", this);
            Destroy(instance);
            return;
        }

        RectTransform instanceRect = instance.GetComponent<RectTransform>();
        if (instanceRect != null)
        {
            Vector3 startPosition = anchor != null ? anchor.position : transform.position;
            instanceRect.position = startPosition + spawnOffset;
        }
        else
        {
            instance.transform.position = anchor != null ? anchor.position : transform.position;
        }

        textComponent.text = value.ToString();
        textComponent.color = textColor;

        StartCoroutine(AnimateText(textComponent, instance.transform, instanceRect));
    }

    private IEnumerator AnimateText(TextMeshProUGUI textComponent, Transform targetTransform, RectTransform targetRect)
    {
        Vector3 startPosition = targetRect != null ? targetRect.position : targetTransform.position;
        Vector3 endPosition = startPosition + new Vector3(0f, moveDistance, 0f);

        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, t);

            if (targetRect != null)
            {
                targetRect.position = currentPosition;
            }
            else
            {
                targetTransform.position = currentPosition;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (targetRect != null)
        {
            targetRect.position = endPosition;
        }
        else
        {
            targetTransform.position = endPosition;
        }

        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            float alpha = 1f - t;
            textComponent.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(targetTransform.gameObject);
    }
}

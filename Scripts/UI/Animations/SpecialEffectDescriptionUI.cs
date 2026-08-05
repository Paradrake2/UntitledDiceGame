using UnityEngine;
using TMPro;
using System.Collections;

public class SpecialEffectDescriptionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private SpecialEffect currentSpecialEffect;
    [SerializeField] private float delayBeforeAnimation = 0f;

    private Coroutine animationRoutine;

    void OnEnable()
    {
        if (AnimationManager.Instance != null)
        {
            AnimationManager.Instance.SpecialEffectAnimationStarted += ShowSpecialEffectDescription;
            AnimationManager.Instance.SpecialEffectAnimationCompleted += HideSpecialEffectDescription;
        }

        if (CombatManager.Instance != null)
        {
            CombatManager.Instance.EnemySelected += SetSpecialEffect;
        }
    }

    void OnDisable()
    {
        if (AnimationManager.Instance != null)
        {
            AnimationManager.Instance.SpecialEffectAnimationStarted -= ShowSpecialEffectDescription;
            AnimationManager.Instance.SpecialEffectAnimationCompleted -= HideSpecialEffectDescription;
        }

        if (CombatManager.Instance != null)
        {
            CombatManager.Instance.EnemySelected -= SetSpecialEffect;
        }

        if (animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
            animationRoutine = null;
        }
    }

    public void ShowSpecialEffectDescription(bool isActive)
    {
        if (!isActive)
        {
            HideSpecialEffectDescription();
            return;
        }

        if (descriptionText == null)
        {
            AnimationManager.Instance?.InvokeSpecialEffectAnimationCompleted();
            return;
        }

        if (currentSpecialEffect == null)
        {
            gameObject.SetActive(false);
            AnimationManager.Instance?.InvokeSpecialEffectAnimationCompleted();
            return;
        }

        descriptionText.text = currentSpecialEffect.EffectDescription;
        descriptionText.rectTransform.localScale = new Vector3(1f, 0f, 1f);
        gameObject.SetActive(true);

        if (animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
        }

        animationRoutine = StartCoroutine(PlayAnimationRoutine());
    }

    private IEnumerator PlayAnimationRoutine()
    {
        var animationManager = AnimationManager.Instance;
        float totalDuration = animationManager != null ? animationManager.specialEffectAnimationDuration : 0f;
        float squishDuration = animationManager != null ? animationManager.specialEffectSquishDuration : 0.25f;

        if (totalDuration <= 0f)
        {
            HideSpecialEffectDescription();
            yield break;
        }

        yield return new WaitForSeconds(delayBeforeAnimation);

        float elapsedTime = 0f;
        Vector3 originalScale = Vector3.one;
        Vector3 squishedScale = new Vector3(1f, 0f, 1f);
        while (elapsedTime < squishDuration)
        {
            float t = elapsedTime / squishDuration;
            descriptionText.rectTransform.localScale = Vector3.Lerp(squishedScale, originalScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        descriptionText.rectTransform.localScale = Vector3.one;

        float visibleDuration = Mathf.Max(0f, totalDuration - squishDuration * 2f);
        yield return new WaitForSeconds(visibleDuration);

        elapsedTime = 0f;
        while (elapsedTime < squishDuration)
        {
            float t = elapsedTime / squishDuration;
            descriptionText.rectTransform.localScale = Vector3.Lerp(originalScale, squishedScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        descriptionText.rectTransform.localScale = Vector3.one;
        gameObject.SetActive(false);
        animationRoutine = null;
        animationManager?.InvokeSpecialEffectAnimationCompleted();
    }

    private void SetSpecialEffect(Enemy enemy)
    {
        if (enemy == null || enemy.SpecialEffect == null)
        {
            currentSpecialEffect = null;
            gameObject.SetActive(false);
            return;
        }

        currentSpecialEffect = enemy.SpecialEffect;
    }

    public void HideSpecialEffectDescription()
    {
        if (animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
            animationRoutine = null;
        }

        if (descriptionText != null)
        {
            descriptionText.rectTransform.localScale = Vector3.one;
        }

        gameObject.SetActive(false);
    }
}

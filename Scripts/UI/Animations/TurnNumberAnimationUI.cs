using UnityEngine;
using TMPro;
using System.Collections;

public class TurnNumberAnimationUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private float delayBeforeAnimation = 0.25f;
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private float squishDuration = 0.25f;

    private Coroutine animationRoutine;

    private void Awake()
    {
        if (turnNumberText != null)
        {
            turnNumberText.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        CombatManager.Instance.NewTurnStarted += PlayAnimation;
        
        AnimationManager.Instance.SpecialEffectAnimationCompleted += OnSpecialEffectAnimationCompleted;

    }

    private void OnDisable()
    {
        CombatManager.Instance.NewTurnStarted -= PlayAnimation;
        AnimationManager.Instance.SpecialEffectAnimationCompleted -= OnSpecialEffectAnimationCompleted;
        

        if (animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
            animationRoutine = null;
        }
    }

    private void PlayAnimation(int turnNumber)
    {

        if (animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
        }

        ResetSize();
        turnNumberText.text = $"Turn {turnNumber}";
        turnNumberText.gameObject.SetActive(true);
        animationRoutine = StartCoroutine(PlayAnimationRoutine(turnNumber));
    }

    private void OnSpecialEffectAnimationCompleted()
    {
        if (turnNumberText != null)
        {
            turnNumberText.gameObject.SetActive(true);
        }
    }

    private IEnumerator PlayAnimationRoutine(int turnNumber)
    {
        if (turnNumberText == null)
        {
            yield break;
        }

        turnNumberText.text = $"Turn {turnNumber + 1}";
        turnNumberText.rectTransform.localScale = new Vector3(1f, 0f, 1f);
        turnNumberText.gameObject.SetActive(true);

        yield return new WaitForSeconds(delayBeforeAnimation);

        float elapsedTime = 0f;
        Vector3 originalScale = Vector3.one;
        Vector3 squishedScale = new Vector3(1f, 0f, 1f);
        while (elapsedTime < squishDuration)
        {
            float t = elapsedTime / squishDuration;
            turnNumberText.rectTransform.localScale = Vector3.Lerp(squishedScale, originalScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        turnNumberText.rectTransform.localScale = Vector3.one;
        yield return new WaitForSeconds(animationDuration);

        elapsedTime = 0f;
        while (elapsedTime < squishDuration)
        {
            float t = elapsedTime / squishDuration;
            turnNumberText.rectTransform.localScale = Vector3.Lerp(originalScale, squishedScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        turnNumberText.gameObject.SetActive(false);
        animationRoutine = null;
        AnimationManager.Instance?.InvokeTurnNumberAnimationCompleted();
    }

    private void ResetSize()
    {
        if (turnNumberText != null)
        {
            turnNumberText.rectTransform.localScale = Vector3.one;
        }
    }
}

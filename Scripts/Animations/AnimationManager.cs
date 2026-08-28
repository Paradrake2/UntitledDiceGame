using UnityEngine;
using System.Collections.Generic;
using System;

public interface IAnimation
{
    void PlayAnimation();
}
/**
Order of operations for animations:
1. Intro animation (e.g. "Battle Start") plays at the start of combat.
2. Special Effect description plays
3. Turn number animation plays at the start of each turn. This enables dice rolling and rerolling.
**/
public class AnimationManager : MonoBehaviour
{
    [SerializeField] private GameObject physicalAttackAnimationPrefab;
    [SerializeField] private GameObject magicalAttackAnimationPrefab;
    [SerializeField] private GameObject animationAnchor;
    [SerializeField] private CombatManager cm;
    private static AnimationManager _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        if (cm == null)
        {
            cm = FindFirstObjectByType<CombatManager>();
        }

        if (Application.isPlaying)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public static AnimationManager TryGetInstance()
    {
        if (_instance != null)
        {
            return _instance;
        }

        _instance = FindFirstObjectByType<AnimationManager>();
        return _instance;
    }

    public static AnimationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<AnimationManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("AnimationManager");
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance = obj.AddComponent<AnimationManager>();
                }
            }
            return _instance;
        }
    }
    public event Action IntroAnimationCompleted; // called when the intro animation is completed, enables dice rolling
    public event Action<bool> SpecialEffectAnimationStarted;
    public event Action SpecialEffectAnimationCompleted; // called when the special effect animation is completed
    public event Action TurnNumberAnimationCompleted; // called when the turn number animation is completed, enables dice rolling
    public bool isStartOfBattle;
    public float specialEffectAnimationDuration = 2f; // total duration of the special effect animation
    public float specialEffectSquishDuration = 0.25f; // duration of the final squash transition
    public void InvokeIntroAnimationCompleted()
    {
        IntroAnimationCompleted?.Invoke();
    }
    public void InvokeSpecialEffectAnimationStarted(bool isActive)
    {
        var handler = SpecialEffectAnimationStarted;
        handler?.Invoke(isActive);

        if (handler == null)
        {
            InvokeSpecialEffectAnimationCompleted();
        }
    }

    public void InvokeSpecialEffectAnimationCompleted()
    {
        SpecialEffectAnimationCompleted?.Invoke();
    }
    public void InvokeTurnNumberAnimationCompleted()
    {
        TurnNumberAnimationCompleted?.Invoke();
    }
    void OnEnable()
    {
        if (cm != null)
        {
            cm.EnemyPhysicalAttack += PlayEnemyPhysicalAttackAnimation;
        }
    }
    void OnDisable()
    {
        if (cm != null)
        {
            cm.EnemyPhysicalAttack -= PlayEnemyPhysicalAttackAnimation;
        }
    }
    void PlayEnemyPhysicalAttackAnimation()
    {
        if (physicalAttackAnimationPrefab != null)
        {
            GameObject pa = Instantiate(physicalAttackAnimationPrefab, animationAnchor.transform);
            Destroy(pa, 0.15f);
        }
    }
}

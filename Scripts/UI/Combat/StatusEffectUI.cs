using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatusEffectUI : MonoBehaviour
{
    [SerializeField] private Image effectIcon;
    [SerializeField] private Image bgSprite;
    [SerializeField] private TMP_Text durationText;
    [SerializeField] private StatusEffect statusEffect;
    public void SetStatusEffect(StatusEffect effect, int remainingDuration)
    {
        statusEffect = effect;
        effectIcon.sprite = effect.EffectIcon;
        durationText.text = remainingDuration.ToString();
        SetColor(effect);
    }
    public void UpdateDuration(int remainingDuration)
    {
        durationText.text = remainingDuration.ToString();
    }
    public void SetColor(StatusEffect effect)
    {
        if (effect.TypeOfEffect == TypeOfEffect.Buff)
        {
            bgSprite.color = Color.green; // Buffs are green
        }
        else if (effect.TypeOfEffect == TypeOfEffect.Debuff)
        {
            bgSprite.color = Color.red; // Debuffs are red
        }
        else
        {
            bgSprite.color = Color.white; // Default color for unknown types
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

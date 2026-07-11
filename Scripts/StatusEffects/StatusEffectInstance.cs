/// <summary>
/// Runtime wrapper around a StatusEffect ScriptableObject. Tracks remaining duration for one
/// active instance of an effect on a combatant.
/// </summary>
public class StatusEffectInstance
{
    public StatusEffect Effect { get; }
    public int RemainingDuration { get; private set; }

    public StatusEffectInstance(StatusEffect effect)
    {
        Effect = effect;
        RemainingDuration = effect.duration;
    }

    /// <summary>Decrements duration and returns true when the effect has expired.</summary>
    public bool DecrementDuration()
    {
        RemainingDuration--;
        return RemainingDuration <= 0;
    }
}

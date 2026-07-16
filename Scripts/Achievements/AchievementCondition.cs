using UnityEngine;

/// <summary>
/// Base class for all achievement conditions.
/// Conditions are ScriptableObject assets assigned to an Achievement.
/// They must be stateless where possible; stateful conditions should
/// override ResetState(), which AchievementManager calls on BattleStarted.
/// </summary>
public abstract class AchievementCondition : ScriptableObject
{
    /// <summary>Returns true when the achievement should be unlocked.</summary>
    public abstract bool Evaluate(AchievementStats stats);

    /// <summary>Override to clear runtime state (e.g. streak counters) on battle start.</summary>
    public virtual void ResetState() { }
}

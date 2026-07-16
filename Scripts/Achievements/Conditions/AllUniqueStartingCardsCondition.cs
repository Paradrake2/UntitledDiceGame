using UnityEngine;

/// <summary>
/// Unlocks when all six dice slots have a different card equipped at the start of a battle.
/// </summary>
[CreateAssetMenu(fileName = "AllUniqueStartingCardsCondition", menuName = "Achievements/Conditions/AllUniqueStartingCards")]
public class AllUniqueStartingCardsCondition : AchievementCondition
{
    public override bool Evaluate(AchievementStats stats) => stats.StartedWithAllUniqueCards;
}

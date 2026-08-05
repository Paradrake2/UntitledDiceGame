using UnityEngine;

[CreateAssetMenu(fileName = "TurnNumberAnimation", menuName = "Animations/TurnNumberAnimation")]
public class TurnNumberAnimation : ScriptableObject, IAnimation
{
    public float animationDuration = 1f;
    public float squishDuration = 0.25f;
    public void PlayAnimation()
    {
        // find turnnumberanimationui in scene and call playanimation on it
        TurnNumberAnimationUI tnau = FindFirstObjectByType<TurnNumberAnimationUI>();
    }
}

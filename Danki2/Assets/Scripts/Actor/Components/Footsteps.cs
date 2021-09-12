using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField]
    [EventRef]
    private string footstepEvent = null;

    [SerializeField]
    private Actor actor;

    // We don't want to hear footsteps when highly blended with other animations
    private const float MinWeightThreshold = 0.5f;

    public void Step(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight < MinWeightThreshold) return;

        EventInstance instance = FmodUtils.CreatePositionedInstance(footstepEvent, actor.transform.position);
        instance.start();
        instance.release();
    }
}

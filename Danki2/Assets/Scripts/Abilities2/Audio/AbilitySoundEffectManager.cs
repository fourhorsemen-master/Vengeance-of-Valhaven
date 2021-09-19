using FMOD.Studio;
using UnityEngine;

public class AbilitySoundEffectManager : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    private SerializableGuid CurrentAbilityId => player.AbilityService.CurrentAbilityId;

    private void Start()
    {
        player.AbilityAnimationListener.StartSubject
            .Subscribe(PlayStartEvent);

        player.AbilityAnimationListener.SwingSubject
            .Subscribe(PlaySwingEvent);

        player.AbilityAnimationListener.ImpactSubject
            .Subscribe(PlayImpactEvent);
    }
    private void PlayStartEvent()
    {
        string startEvent = AbilityLookup2.Instance.GetAbilityFmodEvents(CurrentAbilityId).FmodStartEventRef;
        PlayAbilityEvent(startEvent);
    }

    private void PlaySwingEvent()
    {
        string swingEvent = AbilityLookup2.Instance.GetAbilityFmodEvents(CurrentAbilityId).FmodSwingEventRef;
        PlayAbilityEvent(swingEvent);
    }

    private void PlayImpactEvent()
    {
        string impactEvent = AbilityLookup2.Instance.GetAbilityFmodEvents(CurrentAbilityId).FmodImpactEventRef;
        PlayAbilityEvent(impactEvent);
    }

    private void PlayAbilityEvent(string fmodEvent)
    {
        if (string.IsNullOrEmpty(fmodEvent)) return;

        EventInstance eventInstance = FmodUtils.CreatePositionedInstance(fmodEvent, player.AbilitySource);
        eventInstance.start();
        eventInstance.release();
    }
}
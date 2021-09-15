using FMOD.Studio;
using UnityEngine;

public class AbilitySoundEffectManager : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    private void Start()
    {
        player.AbilityService.AbilityEventSubject
            .Where(x => x.CastEvent == CastEvent.Start)
            .Subscribe(PlayStartEvent);

        player.AbilityService.AbilityEventSubject
            .Where(x => x.CastEvent == CastEvent.Swing)
            .Subscribe(PlaySwingEvent);

        player.AbilityService.AbilityEventSubject
            .Where(x => x.CastEvent == CastEvent.Impact)
            .Subscribe(PlayImpactEvent);
    }
    private void PlayStartEvent(AbilityCastInformation castInformation)
    {
        string startEvent = AbilityLookup2.Instance.GetAbilityFmodEvents(castInformation.AbilityId).FmodStartEventRef;
        PlayAbilityEvent(startEvent);
    }

    private void PlaySwingEvent(AbilityCastInformation castInformation)
    {
        string swingEvent = AbilityLookup2.Instance.GetAbilityFmodEvents(castInformation.AbilityId).FmodSwingEventRef;
        PlayAbilityEvent(swingEvent);
    }

    private void PlayImpactEvent(AbilityCastInformation castInformation)
    {
        string impactEvent = AbilityLookup2.Instance.GetAbilityFmodEvents(castInformation.AbilityId).FmodImpactEventRef;
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
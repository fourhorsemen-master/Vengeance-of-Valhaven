using FMOD.Studio;
using UnityEngine;

public class AbilityVocalisationManager : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    private void Start()
    {
        player.AbilityAnimationListener.SwingSubject
            .Subscribe(HandleVocalisation);
    }

    private void HandleVocalisation()
    {
        AbilityVocalisationType fmodVocalisationType = AbilityTypeLookup.Instance.GetAbilityVocalisationType(
            AbilityLookup.Instance.GetAbilityType(player.AbilityService.CurrentAbilityId)
        );
        float fmodParameterValue = AbilityVocalisationValueLookup.LookupTable[fmodVocalisationType];

        EventInstance eventInstance = FmodUtils.CreatePositionedInstance(
            player.VocalisationEvent,
            player.Centre,
            new FmodParameter("size", fmodParameterValue)
        );
        eventInstance.start();
        eventInstance.release();
    }
}

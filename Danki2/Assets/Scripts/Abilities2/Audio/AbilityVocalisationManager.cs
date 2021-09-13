using FMOD.Studio;
using UnityEngine;

public class AbilityVocalisationManager : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    private void Start()
    {
        player.AbilityService.AbilityEventSubject
            .Where(x => x.CastEvent == CastEvent.Cast)
            .Subscribe(HandleVocalisation);
    }

    private void HandleVocalisation(AbilityCastInformation abilityCastInformation)
    {
        AbilityVocalisationType fmodVocalisationType = AbilityLookup2.Instance.GetAbilityVocalisationType(abilityCastInformation.AbilityId);
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

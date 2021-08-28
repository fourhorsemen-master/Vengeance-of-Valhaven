using System;
using UnityEngine;

public class AbilityVfxManager : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    [SerializeField]
    private float offsetIncrement = 0;

    private void Start()
    {
        player.AbilityService.AbilityCastSubject.Subscribe(HandleVfx);
    }

    private void HandleVfx(AbilityCastInformation abilityCastInformation)
    {
        float offset = 0;
        
        abilityCastInformation.Empowerments.ForEach(empowerment =>
        {
            CreateVFX(abilityCastInformation, empowerment, offset);
            offset += offsetIncrement;
        });
    }

    private void CreateVFX(AbilityCastInformation abilityCastInformation, Empowerment empowerment, float offset)
    {
        switch (abilityCastInformation.Type)
        {
            case AbilityType2.Slash:
            case AbilityType2.Thrust:
                SlashObject.Create(
                    player.AbilitySource,
                    abilityCastInformation.CastRotation * Quaternion.Euler(0, offset, 0),
                    EmpowermentLookup.Instance.GetColour(empowerment)
                );
                break;
            case AbilityType2.Smash:
                SmashObject.Create(
                    player.transform.position,
                    rotation: Quaternion.Euler(0, offset, 0),
                    colour: EmpowermentLookup.Instance.GetColour(empowerment)
                );
                break;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class AbilityCastInformation
{
    public SerializableGuid AbilityId { get; }
    public bool HasDealtDamage { get; }
    public List<Empowerment> Empowerments { get; }
    public Quaternion CastRotation { get; }
    public CastEvent CastEvent { get; }

    public AbilityCastInformation(
        SerializableGuid abilityId,
        bool hasDealtDamage,
        List<Empowerment> empowerments,
        Quaternion castRotation,
        CastEvent castEvent)
    {
        AbilityId = abilityId;
        HasDealtDamage = hasDealtDamage;
        Empowerments = empowerments;
        CastRotation = castRotation;
        CastEvent = castEvent;
    }
}

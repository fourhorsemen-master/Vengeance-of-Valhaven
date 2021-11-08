using System.Collections.Generic;
using UnityEngine;

public class CastContext
{
    public CastContext(
        SerializableGuid abilityId,
        AbilityType abilityType,
        Quaternion castRotation,
        Vector3 collisionTemplateOrigin,
        List<Empowerment> empowerments,
        List<Empowerment> nextEmpowerments
    )
    {
        AbilityId = abilityId;
        AbilityType = abilityType;
        CastRotation = castRotation;
        CollisionTemplateOrigin = collisionTemplateOrigin;
        Empowerments = empowerments;
        NextEmpowerments = nextEmpowerments;
    }

    public SerializableGuid AbilityId { get; }
    public AbilityType AbilityType { get; }
    public Quaternion CastRotation { get; }
    public Vector3 CollisionTemplateOrigin { get; }
    public List<Empowerment> Empowerments { get; }
    public List<Empowerment> NextEmpowerments { get; }
}

using System.Collections.Generic;
using UnityEngine;

public class CastContext
{
    public CastContext(
        Ability ability,
        Quaternion castRotation,
        Vector3 collisionTemplateOrigin,
        List<Empowerment> empowerments,
        List<Empowerment> nextEmpowerments
    )
    {
        Ability = ability;
        CastRotation = castRotation;
        CollisionTemplateOrigin = collisionTemplateOrigin;
        Empowerments = empowerments;
        NextEmpowerments = nextEmpowerments;
    }

    public Ability Ability { get; }
    public Quaternion CastRotation { get; }
    public Vector3 CollisionTemplateOrigin { get; }
    public List<Empowerment> Empowerments { get; }
    public List<Empowerment> NextEmpowerments { get; }
}

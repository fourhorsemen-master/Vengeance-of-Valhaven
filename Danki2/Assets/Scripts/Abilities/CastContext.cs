using System.Collections.Generic;
using UnityEngine;

public class CastContext
{
    public CastContext(
        Ability ability,
        Quaternion castRotation,
        List<Empowerment> empowerments,
        List<Empowerment> nextEmpowerments
    )
    {
        Ability = ability;
        CastRotation = castRotation;
        Empowerments = empowerments;
        NextEmpowerments = nextEmpowerments;
    }

    public Ability Ability { get; }
    public Quaternion CastRotation { get; }
    public List<Empowerment> Empowerments { get; }
    public List<Empowerment> NextEmpowerments { get; }
}

using System.Collections.Generic;
using UnityEngine;

public class AbilityCastInformation
{
    public AbilityType2 Type { get; }
    public bool HasDealtDamage { get; }
    public List<Empowerment> Empowerments { get; }
    public Quaternion CastRotation { get; }

    public AbilityCastInformation(
        AbilityType2 type,
        bool hasDealtDamage,
        List<Empowerment> empowerments,
        Quaternion castRotation
    )
    {
        Type = type;
        HasDealtDamage = hasDealtDamage;
        Empowerments = empowerments;
        CastRotation = castRotation;
    }
}

using System;
using UnityEngine;

public class InstantCastService
{
    private readonly Actor actor;
    
    public InstantCastService(Actor actor)
    {
        this.actor = actor;
    }

    public bool TryGetInstantCast(AbilityReference abilityReference, out InstantCast instantCast)
    {
        return AbilityLookup.TryGetInstantCast(abilityReference, actor, out instantCast);
    }

    public void Cast(InstantCast instantCast, Vector3 target)
    {
        instantCast.Cast(target);
    }
}

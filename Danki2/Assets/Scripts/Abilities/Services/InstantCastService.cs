using UnityEngine;

public class InstantCastService
{
    private readonly Actor actor;
    
    public InstantCastService(Actor actor)
    {
        this.actor = actor;
    }
    
    public void Cast(AbilityReference abilityReference, Vector3 target)
    {
        if (AbilityLookup.TryGetInstantCast(abilityReference, actor, out InstantCast instantCast))
        {
            instantCast.Cast(target);
        }
    }
}

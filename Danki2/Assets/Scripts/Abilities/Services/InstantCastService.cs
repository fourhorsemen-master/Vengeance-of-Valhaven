using UnityEngine;

public class InstantCastService
{
    private readonly Actor actor;
    
    public InstantCastService(Actor actor)
    {
        this.actor = actor;
    }
    
    public void Cast(AbilityReference abilityReference, Vector3 targetPosition)
    {
        AbilityContext abilityContext = new AbilityContext(actor, targetPosition);

        if (AbilityLookup.TryGetInstantCast(abilityReference, abilityContext, out InstantCast instantCast))
        {
            instantCast.Cast();
        }
    }
}

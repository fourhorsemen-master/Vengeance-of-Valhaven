using UnityEngine;

public class InstantCastService : AbilityService
{
    public InstantCastService(Actor owner) : base(owner)
    {
    }

    public void Cast(InstantCast instantCast, Vector3 targetPosition)
    {
        instantCast.Cast(GetAbilityContext(targetPosition));
    }
}

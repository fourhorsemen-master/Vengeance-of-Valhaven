using UnityEngine;

public abstract class AbilityService
{
    protected readonly Actor owner;

    protected AbilityService(Actor owner)
    {
        this.owner = owner;
    }

    protected AbilityContext GetAbilityContext(Vector3 targetPosition)
    {
        return new AbilityContext(owner, targetPosition);
    }
}

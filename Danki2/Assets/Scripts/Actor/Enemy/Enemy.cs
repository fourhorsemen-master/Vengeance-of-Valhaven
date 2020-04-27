using UnityEngine;

public abstract class Enemy : Actor
{
    public Subject OnTelegraph { get; private set; } = new Subject();

    protected virtual void Start()
    {
        this.gameObject.tag = Tags.Enemy;
    }

    public void WaitAndCast(float waitTime, AbilityReference abilityReference, Vector3 targetPosition)
    {
        OnTelegraph.Next();

        InterruptableAction(waitTime, InterruptionType.Hard, () =>
        {
            if (InstantCastService.TryGetInstantCast(abilityReference, out InstantCast instantCast))
            {
                InstantCastService.Cast(instantCast, targetPosition);
            }
        });
    }
}

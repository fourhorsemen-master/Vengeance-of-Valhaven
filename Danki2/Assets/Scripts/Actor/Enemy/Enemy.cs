using System;
using UnityEngine;

public abstract class Enemy : Actor
{
    public Subject OnTelegraph { get; private set; } = new Subject();

    protected virtual void Start()
    {
        this.gameObject.tag = Tags.Enemy;
    }

    public void WaitAndCast(float waitTime, AbilityReference abilityReference, Func<Vector3> positionFinder)
    {
        OnTelegraph.Next();

        InterruptableAction(waitTime, InterruptionType.Hard, () =>
        {
            InstantCastService.Cast(abilityReference, positionFinder());
        });
    }
}

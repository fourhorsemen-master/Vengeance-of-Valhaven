using System;
using UnityEngine;

public abstract class Enemy : Actor
{
    public Subject<float> OnTelegraph { get; private set; } = new Subject<float>();

    protected virtual void Start()
    {
        this.gameObject.tag = Tags.Enemy;
    }

    public void WaitAndCast(float waitTime, AbilityReference abilityReference, Func<Vector3> targeter)
    {
        OnTelegraph.Next(waitTime);

        MovementManager.Stun(waitTime);

        InterruptableAction(waitTime, InterruptionType.Hard, () =>
        {
            InstantCastService.Cast(abilityReference, targeter());
        });
    }
}

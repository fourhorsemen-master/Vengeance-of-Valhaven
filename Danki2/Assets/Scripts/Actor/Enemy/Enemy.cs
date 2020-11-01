using System;
using UnityEngine;

public abstract class Enemy : Actor
{
    public Subject<float> OnTelegraph { get; } = new Subject<float>();

    public BehaviourSubject<bool> PlayerTargeted { get; } = new BehaviourSubject<bool>(false);

    protected override void Start()
    {
        base.Start();
        
        gameObject.tag = Tags.Enemy;
    }

    protected void WaitAndCast(
        float waitTime,
        AbilityReference abilityReference,
        Func<Vector3> targeter,
        Action castCallback = null)
    {
        OnTelegraph.Next(waitTime);

        MovementManager.Pause(waitTime);

        InterruptableAction(waitTime, InterruptionType.Hard, () =>
        {
            InstantCastService.Cast(abilityReference, targeter());
            castCallback?.Invoke();
        });
    }
}

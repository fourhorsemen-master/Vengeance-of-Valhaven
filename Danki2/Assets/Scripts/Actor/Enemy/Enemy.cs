using System;
using UnityEngine;

public abstract class Enemy : Actor
{
    public Subject<float> OnTelegraph { get; private set; } = new Subject<float>();

    public BehaviourSubject<bool> PlayerTargeted { get; } = new BehaviourSubject<bool>(false);

    protected override void Start()
    {
        base.Start();
        
        gameObject.tag = Tags.Enemy;
    }

    public void WaitAndCast(
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

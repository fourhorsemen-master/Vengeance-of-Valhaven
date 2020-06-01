using System;
using UnityEngine;

public abstract class Enemy : Actor, ITargetable
{
    public static readonly Color HighlightedColor = new Color(0.02f, 0.02f, 0.02f);

    public Subject<float> OnTelegraph { get; private set; } = new Subject<float>();

    public BehaviourSubject<bool> PlayerTargeted { get; } = new BehaviourSubject<bool>(false);

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

    protected override void OnDeath()
    {
        base.OnDeath();

        PlayerTargeted.Next(false);
    }
}

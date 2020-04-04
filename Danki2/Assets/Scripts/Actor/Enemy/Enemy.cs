using System;
using UnityEngine;

public abstract class Enemy : Actor
{
    public Subject OnTelegraph { get; private set; } = new Subject();

    protected override void Start()
    {
        base.Start();

        this.gameObject.tag = Tags.Enemy;
    }

    public void WaitAndCast(float waitTime, InstantCast instantCast, Vector3 targetPosition)
    {
        OnTelegraph.Next();

        this.WaitAndAct(waitTime, () => Cast(instantCast, targetPosition));
    }
}

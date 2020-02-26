using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : Actor
{
    public Subject OnTelegraph { get; private set; } = new Subject();

    protected override void Start()
    {
        base.Start();

        this.gameObject.tag = Tags.Enemy;
    }

    public void WaitAndCast(float waitTime, Func<InstantCast> abilityFactory)
    {
        OnTelegraph.Next();

        StartCoroutine(
            WaitAndCastCoroutine(waitTime, abilityFactory)
        );
    }

    private IEnumerator WaitAndCastCoroutine(float waitTime, Func<InstantCast> abilityFactory)
    {
        yield return new WaitForSeconds(waitTime);
        abilityFactory.Invoke().Cast();
    }
}

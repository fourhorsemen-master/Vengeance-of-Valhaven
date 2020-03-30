using System;

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

        this.WaitAndAct(waitTime, () => abilityFactory.Invoke().Cast());
    }
}

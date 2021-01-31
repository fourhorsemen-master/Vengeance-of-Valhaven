using System;
using UnityEngine;

public abstract class Enemy : Actor
{
    public Subject<Tuple<float, Color>> OnTelegraph { get; } = new Subject<Tuple<float, Color>>();

    public BehaviourSubject<bool> PlayerTargeted { get; } = new BehaviourSubject<bool>(false);

    protected override void Start()
    {
        base.Start();
        
        gameObject.tag = Tags.Enemy;
    }

    public void Telegraph(float telegraphTime, Color colour)
    {
        OnTelegraph.Next(new Tuple<float, Color> (telegraphTime, colour));
    }
}

﻿using UnityEngine;

public class Pounce : Channel
{
    public Pounce(AbilityContext context) : base(context)
    {
    }

    public override float Duration => 0.5f;

    public static float Range => 5f;

    public override void Start()
    {
        Debug.Log("Pounce start");
        Context.Owner.LockMovement(Duration,
            0f,
            (Context.TargetPosition - Context.Owner.transform.position)
        );
    }

    public override void Continue()
    {
        Debug.Log("Pounce continuing");
    }

    public override void Cancel()
    {
        Debug.Log("Pounce cancelled");
    }

    public override void End()
    {
        Debug.Log("Pounce end");
        Context.Owner.LockMovement(0.5f,
            Context.Owner.GetStat(Stat.Speed) * 2f,
            (Context.TargetPosition - Context.Owner.transform.position),
            true
        );
    }
}

﻿using UnityEngine;

[Ability(AbilityReference.Backstab)]
public class Backstab : InstantCast
{
    private const float Range = 3f;
    private const float PauseDuration = 0.3f;

    public Backstab(AbilityConstructionArgs arguments) : base(arguments) { }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Swing(offsetTargetPosition);
    }

    public override void Cast(Actor target)
    {
        Swing(target.Centre);

        if (!InRange(target)) return;

        bool backTurned = Vector3.Dot(target.transform.forward, Owner.transform.position - target.transform.position) < 0;

        if (backTurned)
        {
            DealPrimaryDamage(target);
        }
        else
        {
            DealSecondaryDamage(target);
        }        

        CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
    }

    private BackstabObject Swing(Vector3 target)
    {
        Owner.MovementManager.LookAt(target);
        Owner.MovementManager.Pause(PauseDuration);

        Vector3 castDirection = target - Owner.Centre;
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        return BackstabObject.Create(Owner.AbilitySource, castRotation);
    }

    private bool InRange(Actor target)
    {
        bool opposesCaster = Owner.Opposes(target);
        bool closeEnough = Vector3.Distance(target.transform.position, Owner.transform.position) < Range;        

        return opposesCaster && closeEnough;
    }
}
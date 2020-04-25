﻿using System;
using UnityEngine;

class DaggerThrow : InstantCast
{
    private const float DaggerSpeed = 20f;
    private const int ImpactDamage = 2;
    private const int TickDamage = 1;
    private const float DamageTickInterval = 1f;
    private const float DotDuration = 3f;
    private static readonly Vector3 positionTransform = new Vector3(0, 1.25f, 0);

    public DaggerThrow(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Vector3 position = Context.Owner.transform.position + positionTransform;
        Vector3 target = Context.TargetPosition;
        Quaternion rotation = Quaternion.LookRotation(target - position);
        DaggerObject.Fire(Context.Owner, OnCollision, DaggerSpeed, position, rotation);
    }

    private void OnCollision(GameObject gameObject)
    {
        if (gameObject.IsActor())
        {
            Actor actor = gameObject.GetComponent<Actor>();

            if (!actor.Opposes(Context.Owner))
            {
                SuccessFeedbackSubject.Next(false);
                return;
            }

            int strength = Context.Owner.GetStat(Stat.Power);

            actor.ModifyHealth(-ImpactDamage);
            actor.EffectManager.AddActiveEffect(new DOT(TickDamage, DamageTickInterval), DotDuration);

            SuccessFeedbackSubject.Next(true);
        }
        else
        {
            SuccessFeedbackSubject.Next(false);
        }
    }
}

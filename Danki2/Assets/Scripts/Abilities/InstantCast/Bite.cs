﻿using System;
using UnityEngine;
public class Bite : InstantCast
{
    public const int Damage = 5;
    public const float Range = 2f;
    private const float DelayBeforeDamage = 0.75f;
    private const float PauseDuration = 0.3f;

    public Bite(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {  
        Actor owner = Context.Owner;
        Vector3 position = owner.transform.position;
        Vector3 targetPosition = Context.TargetPosition;
        targetPosition.y = 0f;

        owner.InterruptableAction(
            DelayBeforeDamage,
            InterruptionType.Hard,
            () =>
            {
                bool hasDealtDamage = false;

                CollisionTemplateManager.Instance.GetCollidingActors(
                    CollisionTemplate.Wedge90,
                    Range,
                    position,
                    Quaternion.LookRotation(targetPosition - position)
                ).ForEach(actor =>
                {
                    if (owner.Opposes(actor))
                    {
                        owner.DamageTarget(actor, Damage);
                        hasDealtDamage = true;
                    }
                });

                SuccessFeedbackSubject.Next(hasDealtDamage);
            }
        );

        BiteObject.Create(owner.transform);
        owner.MovementManager.Stun(DelayBeforeDamage + PauseDuration);
    }
}

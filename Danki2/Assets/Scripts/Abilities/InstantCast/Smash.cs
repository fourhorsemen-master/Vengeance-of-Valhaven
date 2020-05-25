﻿using UnityEngine;

[Ability(AbilityReference.Smash, new []{"PerfectSmash"})]
public class Smash : InstantCast
{
    private const float DistanceFromCaster = 1f;
    private const float Radius = 1f;

    public Smash(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        target.y = 0;

        Vector3 directionToTarget = target == position ? Vector3.right : (target - position).normalized;
        Vector3 center = position + (directionToTarget * DistanceFromCaster);

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Cylinder,
            Radius,
            center
        ).ForEach(actor =>
        {
            if (Owner.Opposes(actor))
            {
                DealPrimaryDamage(actor);
                hasDealtDamage = true;

                if (HasBonus("PerfectSmash")) actor.EffectManager.AddActiveEffect(new Stun(3), 3);
            }
        });

        CustomCamera.Instance.AddShake(ShakeIntensity.High);
        SmashObject.Create(center);

        SuccessFeedbackSubject.Next(hasDealtDamage);
    }
}

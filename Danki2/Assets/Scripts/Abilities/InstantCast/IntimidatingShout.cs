﻿using System.Collections.Generic;
using UnityEngine;

[Ability(AbilityReference.IntimidatingShout)]
public class IntimidatingShout : InstantCast
{
    private const float Range = 4;
    private const int DefenceModification = 2;
    private const float Duration = 6;

    public IntimidatingShout(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        List<Actor> collidingActors = GetCollidingActors();
        SuccessFeedbackSubject.Next(collidingActors.Count > 0);
        collidingActors.ForEach(ReduceDefence);

        IntimidatingShoutObject.Create(Owner.transform);
        CustomCamera.Instance.AddShake(ShakeIntensity.High);
    }

    private List<Actor> GetCollidingActors()
    {
        return CollisionTemplateManager.Instance
            .GetCollidingActors(CollisionTemplate.Cylinder, Range, Owner.transform.position)
            .Where(actor => Owner.Opposes(actor));
    }

    private void ReduceDefence(Actor actor)
    {
        // Just commenting out as this ability will be removed later anyway
        // actor.EffectManager.AddActiveEffect(new DefenceDebuff(DefenceModification), Duration);
    }
}

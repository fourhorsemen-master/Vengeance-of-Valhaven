﻿using UnityEngine;

[Ability(AbilityReference.Rend)]
public class Rend : Cast
{
    private const float Range = 3f;
    private const int BleedStacks = 2;

    public Rend(AbilityConstructionArgs arguments) : base(arguments) { }

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Owner.MovementManager.LookAt(floorTargetPosition);

        bool enemiesHit = false;

        TemplateCollision(
            CollisionTemplateShape.Cylinder,
            Range,
            Owner.CollisionTemplateSource,
            Quaternion.identity,
            actor =>
            {
                actor.EffectManager.AddStacks(StackingEffect.Bleed, BleedStacks);
                enemiesHit = true;
            },
            CollisionSoundLevel.Low
        );

        RendObject.Create(Owner.transform, Owner.AbilitySource);

        if (enemiesHit) CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
    }
}

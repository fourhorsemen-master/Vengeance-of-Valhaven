﻿using UnityEngine;

[Ability(AbilityReference.Maul)]
public class Maul : InstantCast
{
    private const int TotalBiteCount = 3;
    private const float BiteInterval = 0.5f;
    private const float BiteRange = 2.5f;

    private Direction direction = Direction.Left;
    private int biteCount = 0;

    public Maul(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Vector3 castDirection = floorTargetPosition - Owner.transform.position;

        Owner.MovementManager.LookAt(floorTargetPosition);

        MaulObject maulObject = MaulObject.Create(Owner.AbilitySource);

        Owner.InterruptibleIntervalAction(BiteInterval, InterruptionType.Hard, () => Bite(castDirection, maulObject), 0f, TotalBiteCount);
    }

    private void Bite(Vector3 castDirection, MaulObject maulObject)
    {
        biteCount++;

        Vector3 horizontalDirection = Vector3.Cross(castDirection, Vector3.up).normalized;
        int directionMultiplier = direction == Direction.Right ? 1 : -1;
        Vector3 randomisedcastDirection = castDirection.normalized + horizontalDirection * 0.25f * directionMultiplier;

        Quaternion castRotation = GetMeleeCastRotation(randomisedcastDirection);

        maulObject.Bite(castRotation);

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(CollisionTemplate.Wedge45, BiteRange, Owner.CollisionTemplateSource, castRotation)
            .Where(actor => Owner.Opposes(actor))
            .ForEach(actor =>
            {
                DealPrimaryDamage(actor);
                actor.EffectManager.AddStack(StackingEffect.Slow);
                hasDealtDamage = true;
            });

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            SuccessFeedbackSubject.Next(true);
        }
        else if (biteCount == TotalBiteCount)
        {
            SuccessFeedbackSubject.Next(false);
        }

        Owner.MovementManager.Pause(BiteInterval);

        direction = direction == Direction.Left ? Direction.Right : Direction.Left;
    }
}

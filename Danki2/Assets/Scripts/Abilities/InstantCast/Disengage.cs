﻿using UnityEngine;

[Ability(AbilityReference.Disengage, new[] { "Parting Shot" })]
public class Disengage : InstantCast
{
    private const float leapSpeed = 14f;
    private const float leapDistance = 6f;
    private const float duration = leapDistance / leapSpeed;
    private const float pauseDuration = 0.3f;

    private const float partingShotRange = 3.2f;

    public Disengage(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Vector3 position = Owner.transform.position;
        Vector3 travelDirection = position - floorTargetPosition;
        Vector3 faceDirection = floorTargetPosition - position;

        Owner.MovementManager.TryLockMovement(MovementLockType.Dash, duration, leapSpeed, travelDirection, faceDirection);
        Owner.StartTrail(duration);

        DisengageObject.Create(Owner.transform, duration);

        Owner.WaitAndAct(duration, () =>
        {
            Owner.MovementManager.Pause(pauseDuration);
        });

        SuccessFeedbackSubject.Next(true);

        if (HasBonus("Parting Shot"))
        {
            SmashObject.Create(position, false);

            CollisionTemplateManager.Instance.GetCollidingActors(
                CollisionTemplate.Cylinder,
                partingShotRange,
                position
            ).ForEach(actor =>
            {
                if (Owner.Opposes(actor))
                {
                    DealPrimaryDamage(actor);
                }
            });
        }
    }
}

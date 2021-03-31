using UnityEngine;

[Ability(AbilityReference.Disengage)]
public class Disengage : InstantCast
{
    private const float leapSpeed = 14f;
    private const float leapDistance = 6f;
    private const float duration = leapDistance / leapSpeed;
    private const float pauseDuration = 0.3f;

    private const float partingShotRange = 2.2f;
    private const float smashObjectScaleFactor = 1.28f;

    public Disengage(AbilityConstructionArgs arguments) : base(arguments) { }

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

        SmashObject.Create(position, smashObjectScaleFactor);

        bool dealtDamage = false;

        TemplateCollision(
            CollisionTemplateShape.Cylinder,
            partingShotRange,
            position,
            Quaternion.identity,
            actor =>
            {
                DealPrimaryDamage(actor);
                dealtDamage = true;
            }
        );

        SuccessFeedbackSubject.Next(dealtDamage);
    }
}

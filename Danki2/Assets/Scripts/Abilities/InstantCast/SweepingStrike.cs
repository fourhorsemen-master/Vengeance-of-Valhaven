using UnityEngine;

[Ability(AbilityReference.SweepingStrike)]
public class SweepingStrike : InstantCast
{
    private const float Range = 2.8f;
    private const float PauseDuration = 0.3f;

    public SweepingStrike(AbilityConstructionArgs arguments) : base(arguments) { }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Vector3 castDirection = floorTargetPosition - Owner.transform.position;
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        bool hasDealtDamage = false;

        TemplateCollision(
            CollisionTemplateShape.Wedge90,
            Range,
            Owner.CollisionTemplateSource,
            castRotation,
            actor =>
            {
                DealPrimaryDamage(actor);
                hasDealtDamage = true;
            },
            CollisionSoundLevel.Low
        );

        SweepingStrikeObject sweepingStrikeObject = SweepingStrikeObject.Create(Owner.AbilitySource, castRotation);

        Owner.MovementManager.LookAt(floorTargetPosition);
        Owner.MovementManager.Pause(PauseDuration);

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }
    }
}

using UnityEngine;

[Ability(AbilityReference.Pounce)]
public class Pounce : InstantCast
{
    // The ai casting this ability should determine cast range
    private const float MinMovementDuration = 0.25f;
    private const float MaxMovementDuration = 0.5f;
    private const float PounceSpeedMultiplier = 3f;
    private const float DamageRadius = 2f;
    private const float PauseDuration = 0.3f;

    public Pounce(AbilityConstructionArgs arguments) : base(arguments) { }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Vector3 position = Owner.transform.position;
        Vector3 direction = floorTargetPosition - position;

        float distance = Vector3.Distance(floorTargetPosition, position);
        float pounceSpeed = Owner.StatsManager.Get(Stat.Speed) * PounceSpeedMultiplier;
        float duration = Mathf.Clamp(distance / pounceSpeed, MinMovementDuration, MaxMovementDuration);

        Owner.MovementManager.TryLockMovement(MovementLockType.Dash, duration, pounceSpeed, direction, direction);
        Owner.StartTrail(duration);

        PounceObject.Create(position, Quaternion.LookRotation(floorTargetPosition - position));

        Owner.InterruptibleAction(duration, InterruptionType.Hard, DamageOnLand);
    }

    private void DamageOnLand()
    {
        bool hasDealtDamage = false;

        TemplateCollision(
            CollisionTemplateShape.Wedge90,
            DamageRadius,
            Owner.CollisionTemplateSource,
            Quaternion.LookRotation(Owner.transform.forward),
            actor =>
            {
                DealPrimaryDamage(actor);
                hasDealtDamage = true;
            }
        );

        BiteObject.Create(Owner.AbilitySource, Quaternion.LookRotation(Owner.transform.forward));

        Owner.MovementManager.Pause(PauseDuration);
        SuccessFeedbackSubject.Next(hasDealtDamage);

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }
    }
}
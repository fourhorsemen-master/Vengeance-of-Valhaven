using UnityEngine;

[Ability(AbilityReference.WraithSwipe)]
public class WraithSwipe : InstantCast
{
    private const float MovementSpeedMultiplier = 6f;
    private const float MovementDuration = 0.1f;
    private const float PauseDuration = 0.3f;
    private const float Range = 2f;
    
    public WraithSwipe(AbilityConstructionArgs arguments) : base(arguments) {}

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Vector3 position = Owner.transform.position;
        Vector3 castDirection = floorTargetPosition - position;

        float lungeSpeed = Owner.StatsManager.Get(Stat.Speed) * MovementSpeedMultiplier;

        Owner.MovementManager.TryLockMovement(
            MovementLockType.Dash,
            MovementDuration,
            lungeSpeed,
            castDirection,
            castDirection
        );

        Owner.StartTrail(MovementDuration + PauseDuration);

        Owner.WaitAndAct(MovementDuration, () => DamageOnLand(castDirection));
    }

    private void DamageOnLand(Vector3 castDirection)
    {
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

        WraithSwipeObject.Create(Owner.AbilitySource, castRotation);
        
        SuccessFeedbackSubject.Next(hasDealtDamage);
        Owner.MovementManager.Pause(PauseDuration);

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }
    }
}

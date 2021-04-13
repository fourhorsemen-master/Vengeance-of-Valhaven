using UnityEngine;

[Ability(AbilityReference.PoisonStab)]
public class PoisonStab : Cast
{
    private const float MovementSpeedMultiplier = 6f;
    private const float MovementDuration = 0.1f;
    private const float PauseDuration = 0.3f;
    private const float Range = 2f;
    private const float PoisonDuration = 10f;

    public PoisonStab(AbilityConstructionArgs arguments) : base(arguments) {}

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
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

        Owner.InterruptibleAction(
            MovementDuration,
            InterruptionType.Hard,
            () => PoisonOnLand(castDirection)
        );
    }

    private void PoisonOnLand(Vector3 castDirection)
    {
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        bool hasAppliedPoison = false;

        TemplateCollision(
            CollisionTemplateShape.Wedge45,
            Range,
            Owner.CollisionTemplateSource,
            castRotation,
            actor =>
            {
                actor.EffectManager.AddActiveEffect(ActiveEffect.Poison, PoisonDuration);
                DealPrimaryDamage(actor);
                hasAppliedPoison = true;
            },
            CollisionSoundLevel.Low
        );

        SuccessFeedbackSubject.Next(hasAppliedPoison);
        Owner.MovementManager.Pause(PauseDuration);

        if (hasAppliedPoison)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }
    }
}

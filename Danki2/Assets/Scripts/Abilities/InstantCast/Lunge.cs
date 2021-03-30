using UnityEngine;

[Ability(AbilityReference.Lunge)]
public class Lunge : InstantCast
{
    private const float MinMovementDuration = 0.1f;
    private const float MaxMovementDuration = 0.2f;
    private const float LungeSpeedMultiplier = 6f;
    private const float StunRange = 2f;
    private const float StunDuration = 0.5f;
    private const float PauseDuration = 0.3f;

    private readonly Subject<Vector3> onFinishMovement = new Subject<Vector3>();

    public Lunge(AbilityContructionArgs arguments) : base(arguments) { }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Vector3 position = Owner.transform.position;
        Vector3 castDirection = floorTargetPosition - position;

        float distance = Vector3.Distance(floorTargetPosition, position);
        float lungeSpeed = Owner.StatsManager.Get(Stat.Speed) * LungeSpeedMultiplier;
        float duration = Mathf.Clamp(distance/lungeSpeed, MinMovementDuration, MaxMovementDuration);

        Owner.MovementManager.TryLockMovement(MovementLockType.Dash, duration, lungeSpeed, castDirection, castDirection);

        LungeObject.Create(Owner.AbilitySource, Quaternion.LookRotation(castDirection), onFinishMovement);
        Owner.StartTrail(duration + PauseDuration);

        Owner.InterruptibleAction(
            duration,
            InterruptionType.Hard,
            () => DamageOnLand(castDirection)
        );
    }

    private void DamageOnLand(Vector3 castDirection)
    {
        onFinishMovement.Next(Owner.AbilitySource);

        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        bool hasDealtDamage = false;

        TemplateCollision(
            CollisionTemplate.Wedge90,
            StunRange,
            Owner.CollisionTemplateSource,
            castRotation,
            actor =>
            {
                actor.EffectManager.AddActiveEffect(ActiveEffect.Stun, StunDuration);
                DealPrimaryDamage(actor);
                hasDealtDamage = true;
            }
        );

        SuccessFeedbackSubject.Next(hasDealtDamage);
        Owner.MovementManager.Pause(PauseDuration);

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }
    }
}

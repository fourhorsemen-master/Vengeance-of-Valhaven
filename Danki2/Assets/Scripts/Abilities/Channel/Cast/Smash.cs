using UnityEngine;

[Ability(AbilityReference.Smash, new []{"PerfectSmash"})]
public class Smash : Cast
{
    private const float DistanceFromCaster = 1.8f;
    private const float Radius = 1.6f;
    private const float PerfectSmashStunDuration = 3f;
    private const float PauseDuration = 0.3f;

    public Smash(AbilityConstructionArgs arguments) : base(arguments) { }

    protected override void Start() => PlayStartEvent();

    protected override void Cancel() => StopStartEvents();

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Owner.MovementManager.LookAt(floorTargetPosition);
        Owner.MovementManager.Pause(PauseDuration);

        PlayEndEvent();

        Vector3 position = Owner.transform.position;

        Vector3 directionToTarget = floorTargetPosition == position
            ? Vector3.right
            : (floorTargetPosition - position).normalized;
        Vector3 center = position + (directionToTarget * DistanceFromCaster);

        bool hasDealtDamage = false;

        TemplateCollision(
            CollisionTemplate.Cylinder,
            Radius,
            center,
            Quaternion.identity,
            actor =>
            {
                DealPrimaryDamage(actor);
                hasDealtDamage = true;

                if (HasBonus("PerfectSmash")) actor.EffectManager.AddActiveEffect(ActiveEffect.Stun, PerfectSmashStunDuration);
            }
        );

        CustomCamera.Instance.AddShake(ShakeIntensity.High);
        SmashObject.Create(center);

        SuccessFeedbackSubject.Next(hasDealtDamage);
    }
}

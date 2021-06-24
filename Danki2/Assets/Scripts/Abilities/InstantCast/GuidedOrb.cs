using UnityEngine;

[Ability(AbilityReference.GuidedOrb)]
public class GuidedOrb : InstantCast
{
    private const float MaxDuration = 8f;
    private const float Speed = 1.5f;
    private const float RotationSpeed = 1f;
    private const float Range = 2.5f;

    public GuidedOrb(AbilityConstructionArgs arguments) : base(arguments) {}

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        SuccessFeedbackSubject.Next(false);
    }

    public override void Cast(Actor target)
    {
        SuccessFeedbackSubject.Next(true);

        GuidedOrbObject.Fire(
            MaxDuration,
            Speed,
            RotationSpeed,
            target.CentreTransform,
            Owner.AbilitySource,
            HandleExplosion,
            Owner.DeathSubject
        );
    }

    private void HandleExplosion(Vector3 position)
    {
        TemplateCollision(
            CollisionTemplateShape.Sphere,
            Range,
            position,
            Quaternion.identity,
            actor =>
            {
                DealPrimaryDamage(actor);
            }
        );

        CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
    }
}

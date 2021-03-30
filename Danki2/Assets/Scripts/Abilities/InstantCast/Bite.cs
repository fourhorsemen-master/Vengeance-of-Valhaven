using UnityEngine;

[Ability(AbilityReference.Bite)]
public class Bite : InstantCast
{
    public const float Range = 2f;
    private const float PauseDuration = 0.3f;

    public Bite(AbilityConstructionArgs arguments) : base(arguments) { }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Vector3 castDirection = floorTargetPosition - Owner.transform.position;
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        BiteObject.Create(Owner.AbilitySource, castRotation);

        Owner.MovementManager.LookAt(floorTargetPosition);
        Owner.MovementManager.Pause(PauseDuration);

        bool hasDealtDamage = false;

        TemplateCollision(
            CollisionTemplate.Wedge45,
            Range,
            Owner.CollisionTemplateSource,
            castRotation,
            actor =>
            {
                DealPrimaryDamage(actor);
                hasDealtDamage = true;
            }
        );

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }

        SuccessFeedbackSubject.Next(hasDealtDamage);
    }
}

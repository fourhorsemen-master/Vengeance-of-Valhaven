using UnityEngine;

[Ability(AbilityReference.Cleave)]
public class Cleave : InstantCast
{
    private const float Range = 4f;
    private const float PauseDuration = 0.75f;
    private const float KnockBackDuration = 0.3f;
    private const float KnockBackSpeed = 8f;

    public Cleave(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] availableBonuses)
        : base(owner, abilityData, fmodStartEvent, fmodEndEvent, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Vector3 castDirection = floorTargetPosition - Owner.transform.position;
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        CleaveObject.Create(Owner.AbilitySource, castRotation);

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(CollisionTemplate.Wedge180, Range, Owner.CollisionTemplateSource, castRotation)
            .Where(actor => Owner.Opposes(actor))
            .ForEach(actor =>
            {
                DealPrimaryDamage(actor);
                KnockBack(actor);
                hasDealtDamage = true;
            });

        SuccessFeedbackSubject.Next(hasDealtDamage);

        Owner.MovementManager.LookAt(floorTargetPosition);
        Owner.MovementManager.Pause(PauseDuration);

        var shakeIntensity = hasDealtDamage ? ShakeIntensity.High : ShakeIntensity.Medium;
        CustomCamera.Instance.AddShake(shakeIntensity);
    }

    private void KnockBack(Actor actor)
    {
        Vector3 knockBackDirection = actor.transform.position - Owner.transform.position;
        Vector3 knockBackFaceDirection = actor.transform.forward;

        actor.MovementManager.TryLockMovement(
            MovementLockType.Knockback,
            KnockBackDuration,
            KnockBackSpeed,
            knockBackDirection,
            knockBackFaceDirection
        );
    }
}

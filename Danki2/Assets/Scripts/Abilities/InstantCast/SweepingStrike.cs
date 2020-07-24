using UnityEngine;

[Ability(AbilityReference.SweepingStrike)]
public class SweepingStrike : InstantCast
{
    private const float Range = 4f;
    private const float PauseDuration = 0.3f;

    private const float knockBackDuration = 0.5f;
    private const float knockBackSpeed = 5f;

    public SweepingStrike(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        Vector3 castDirection = target - Owner.Centre;
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(CollisionTemplate.Wedge90, Range, position, castRotation)
            .Where(actor => Owner.Opposes(actor))
            .ForEach(actor =>
            {
                DealPrimaryDamage(actor);
                hasDealtDamage = true;
                KnockBack(actor);
            });

        SuccessFeedbackSubject.Next(hasDealtDamage);

        SweepingStrikeObject sweepingStrikeObject = SweepingStrikeObject.Create(position, castRotation);

        Owner.MovementManager.LookAt(target);
        Owner.MovementManager.Stun(PauseDuration);

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            sweepingStrikeObject.PlayHitSound();
        }
    }

    private void KnockBack(Actor actor)
    {
        Vector3 knockBackDirection = actor.transform.position - Owner.transform.position;
        Vector3 knockBackFaceDirection = actor.transform.forward;

        actor.MovementManager.TryLockMovement(
            MovementLockType.Knockback,
            knockBackDuration,
            knockBackSpeed,
            knockBackDirection,
            knockBackFaceDirection
        );
    }
}

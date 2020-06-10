using UnityEngine;

[Ability(AbilityReference.SweepingStrike)]
public class SweepingStrike : InstantCast
{
    private const float Range = 4f;
    private const float PauseDuration = 0.3f;

    private const float knockBackDuration = 1f;
    private const float knockBackSpeed = 10f;

    public SweepingStrike(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        Vector3 castDirection = target - position;
        castDirection.y = 0f;

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge90,
            Range,
            position,
            Quaternion.LookRotation(castDirection)
        ).ForEach(actor =>
        {
            if (Owner.Opposes(actor))
            {
                DealPrimaryDamage(actor);
                hasDealtDamage = true;
                KnockBack(actor);
            }
        });

        SuccessFeedbackSubject.Next(hasDealtDamage);

        // SlashObject slashObject = SlashObject.Create(position, Quaternion.LookRotation(castDirection));

        Owner.MovementManager.LookAt(target);
        Owner.MovementManager.Stun(PauseDuration);

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            // slashObject.PlayHitSound();
        }
    }

    public void KnockBack(Actor actor)
    {
        Vector3 knockBackDirection = Owner.transform.position - actor.transform.position;
        Vector3 knockBackFaceDirection = actor.transform.forward;

        actor.MovementManager.LockMovement(
            knockBackDuration,
            knockBackSpeed,
            knockBackDirection,
            knockBackFaceDirection
            );
    }
}

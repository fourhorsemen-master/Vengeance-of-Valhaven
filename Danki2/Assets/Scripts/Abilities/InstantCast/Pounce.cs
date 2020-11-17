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

    public Pounce(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        target.y = position.y;
        Vector3 direction = target - position;

        float distance = Vector3.Distance(target, position);
        float pounceSpeed = Owner.GetStat(Stat.Speed) * PounceSpeedMultiplier;
        float duration = Mathf.Clamp(distance / pounceSpeed, MinMovementDuration, MaxMovementDuration);

        Owner.MovementManager.TryLockMovement(MovementLockType.Dash, duration, pounceSpeed, direction, direction);
        Owner.StartTrail(duration);

        PounceObject.Create(position, Quaternion.LookRotation(target - position));

        Owner.InterruptibleAction(duration, InterruptionType.Hard, DamageOnLand);
    }

    private void DamageOnLand()
    {
        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge90,
            DamageRadius,
            Owner.transform.position,
            Quaternion.LookRotation(Owner.transform.forward)
        ).ForEach(actor =>
        {
            if (Owner.Opposes(actor))
            {
                DealPrimaryDamage(actor);
                hasDealtDamage = true;
            }
        });

        BiteObject.Create(Owner.transform);

        Owner.MovementManager.Pause(PauseDuration);
        SuccessFeedbackSubject.Next(hasDealtDamage);

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }
    }
}
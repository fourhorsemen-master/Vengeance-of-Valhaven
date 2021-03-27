using UnityEngine;

[Ability(AbilityReference.Swipe)]
public class Swipe : InstantCast
{
    private const float DashDuration = 0.2f;
    private const float DashSpeedMultiplier = 1.5f;
    private const float PauseDuration = 0.3f;
    private const float DamageRadius = 3f;

    public Swipe(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] availableBonuses)
        : base(owner, abilityData, fmodStartEvent, fmodEndEvent, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Owner.MovementManager.LookAt(floorTargetPosition);

        Owner.MovementManager.TryLockMovement(
            MovementLockType.Dash,
            DashDuration,
            Owner.StatsManager.Get(Stat.Speed) * DashSpeedMultiplier,
            Owner.transform.forward,
            Owner.transform.forward
        );

        Owner.InterruptibleAction(DashDuration, InterruptionType.Hard, DamageOnLand);
    }

    private void DamageOnLand()
    {
        bool hasDealtDamage = false;

        TemplateCollision(
            CollisionTemplateShape.Wedge90,
            DamageRadius,
            Owner.CollisionTemplateSource,
            Quaternion.LookRotation(Owner.transform.forward),
            actor =>
            {
                DealPrimaryDamage(actor);
                hasDealtDamage = true;
            }
        );

        SwipeObject swipeObject = SwipeObject.Create(
            Owner.AbilitySource,
            GetMeleeCastRotation(Owner.transform.forward)
        );

        Owner.MovementManager.Pause(PauseDuration);
        SuccessFeedbackSubject.Next(hasDealtDamage);

        if (hasDealtDamage) CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
    }
}

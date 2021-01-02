using UnityEngine;

[Ability(AbilityReference.Swipe)]
public class Swipe : InstantCast
{
    private const float DashDuration = 0.2f;
    private const float DashSpeedMultiplier = 3f;
    private const float PauseDuration = 0.3f;
    private const float DamageRadius = 3f;

    public Swipe(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Owner.MovementManager.LookAt(floorTargetPosition);

        Owner.MovementManager.TryLockMovement(MovementLockType.Dash, DashDuration, Owner.StatsManager.Get(Stat.Speed) * DashSpeedMultiplier, Owner.transform.forward, Owner.transform.forward);

        Owner.InterruptibleAction(DashDuration, InterruptionType.Hard, DamageOnLand);
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

        var swipeObject = SwipeObject.Create(Owner.Centre, GetMeleeCastRotation(Owner.transform.forward));

        Owner.MovementManager.Pause(PauseDuration);
        SuccessFeedbackSubject.Next(hasDealtDamage);

        if (hasDealtDamage)
        {
            swipeObject.PlayHitSound();
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }
    }
}

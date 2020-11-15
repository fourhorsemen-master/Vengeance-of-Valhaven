using UnityEngine;

[Ability(AbilityReference.Dash)]
public class Dash : InstantCast
{
    private const float DashSpeedMultiplier = 5f;
    private const float DashDuration = 0.2f;

    public Dash(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 _)
    {
        Vector3 position = Owner.transform.position;
        Vector3 direction = floorTargetPosition - position;

        Owner.MovementManager.TryLockMovement(
            MovementLockType.Dash,
            DashDuration,
            Owner.GetStat(Stat.Speed) * DashSpeedMultiplier,
            direction,
            direction
        );

        DashObject.Create(Owner.transform);

        SuccessFeedbackSubject.Next(true);
    }
}

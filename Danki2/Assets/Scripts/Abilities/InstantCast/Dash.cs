using UnityEngine;

[Ability(AbilityReference.Dash)]
public class Dash : InstantCast
{
    private const float DashSpeedMultiplier = 4f;
    private const float DashDuration = 0.15f;

    public Dash(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        target.y = position.y;
        Vector3 direction = target - position;

        Owner.MovementManager.LockMovement(
            DashDuration,
            Owner.GetStat(Stat.Speed) * DashSpeedMultiplier,
            direction,
            direction
        );

        DashObject.Create(Owner.transform);

        SuccessFeedbackSubject.Next(true);
    }
}

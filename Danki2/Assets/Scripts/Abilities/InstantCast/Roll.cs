using System.Collections.Generic;
using UnityEngine;

[Ability(AbilityReference.Roll)]
public class Roll : InstantCast
{
    private const float RollSpeedMultiplier = 4f;
    private const float RollDuration = 0.15f;

    public Roll(Actor owner, AbilityData abilityData) : base(owner, abilityData)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        target.y = position.y;
        Vector3 direction = target - position;

        Owner.MovementManager.LockMovement(
            RollDuration,
            Owner.GetStat(Stat.Speed) * RollSpeedMultiplier,
            direction,
            direction
        );

        RollObject.Create(Owner.transform);

        SuccessFeedbackSubject.Next(true);
    }
}

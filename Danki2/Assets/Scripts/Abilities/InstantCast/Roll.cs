using System;
using UnityEngine;

public class Roll : InstantCast
{
    public static readonly AbilityData BaseAbilityData = new AbilityData(0, 0, 0);

    private const float RollSpeedMultiplier = 4f;
    private const float RollDuration = 0.15f;

    public Roll(AbilityContext context, AbilityData abilityData) : base(context, abilityData)
    {
    }

    public override void Cast()
    {
        Actor owner = Context.Owner;
        Vector3 position = owner.transform.position;
        Vector3 target = Context.TargetPosition;
        target.y = position.y;
        Vector3 direction = target - position;

        owner.MovementManager.LockMovement(
            RollDuration,
            owner.GetStat(Stat.Speed) * RollSpeedMultiplier,
            direction,
            direction
        );

        RollObject.Create(owner.transform);

        SuccessFeedbackSubject.Next(true);
    }
}

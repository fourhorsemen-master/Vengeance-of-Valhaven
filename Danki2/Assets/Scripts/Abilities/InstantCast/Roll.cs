using System;
using UnityEngine;

public class Roll : InstantCast
{
    private const float RollSpeedMultiplier = 4f;
    private const float RollDuration = 0.15f;

    public Roll(AbilityContext context, Action<bool> completionCallback)
        : base(context, completionCallback)
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

        this.isSuccessfulCallback(true);
    }
}

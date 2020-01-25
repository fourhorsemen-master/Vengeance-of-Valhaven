using UnityEngine;

public class Roll : InstantCast
{
    public Roll(AbilityContext context) : base(context)
    {
    }

    private static readonly float rollSpeedMultiplier = 4f;
    private static readonly float rollDuration = 0.15f;

    public override void Cast()
    {
        Actor caster = Context.Owner;

        Vector3 targetPosition = Context.TargetPosition - caster.transform.position;
        targetPosition.y = caster.transform.position.y;

        caster.LockMovement(
            rollDuration,
            caster.GetStat(Stat.Speed) * rollSpeedMultiplier,
            targetPosition,
            passThrough: true
        );
    }
}

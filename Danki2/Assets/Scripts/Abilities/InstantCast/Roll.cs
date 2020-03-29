using UnityEngine;

public class Roll : InstantCast
{
    private const float RollSpeedMultiplier = 4f;
    private const float RollDuration = 0.15f;

    public Roll(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Actor caster = Context.Owner;

        Vector3 targetPosition = Context.TargetPosition - caster.transform.position;
        targetPosition.y = caster.transform.position.y;

        caster.LockMovement(
            RollDuration,
            caster.GetStat(Stat.Speed) * RollSpeedMultiplier,
            targetPosition,
            passThrough: true
        );

        RollObject.Create(caster.transform);
    }
}

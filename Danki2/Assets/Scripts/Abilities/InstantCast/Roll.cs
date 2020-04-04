using UnityEngine;

public class Roll : InstantCast
{
    private const float RollSpeedMultiplier = 4f;
    private const float RollDuration = 0.15f;

    public override AbilityReference AbilityReference => AbilityReference.Roll;

    public override void Cast(AbilityContext context)
    {
        Actor caster = context.Owner;

        var position = caster.transform.position;
        Vector3 targetPosition = context.TargetPosition - position;
        targetPosition.y = position.y;

        caster.LockMovement(
            RollDuration,
            caster.GetStat(Stat.Speed) * RollSpeedMultiplier,
            targetPosition,
            passThrough: true
        );

        RollObject.Create(caster.transform);
    }
}

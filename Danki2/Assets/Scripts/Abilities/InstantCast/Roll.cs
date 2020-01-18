using UnityEngine;

public class Roll : InstantCast
{
    public Roll(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Actor caster = Context.Owner;

        Vector3 targetPosition = Context.TargetPosition - caster.transform.position;
        targetPosition.y = caster.transform.position.y;

        caster.LockMovement(
            0.15f,
            caster.GetStat(Stat.Speed) * 4,
            targetPosition,
            passThrough: true
        );
    }
}

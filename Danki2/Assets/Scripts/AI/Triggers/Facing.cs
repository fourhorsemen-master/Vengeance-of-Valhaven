using UnityEngine;

public class Facing : StateMachineTrigger
{
    private readonly IMovementManager self;
    private readonly Actor target;
    private readonly float? maxHorizontalAngleDegrees;

    public Facing(IMovementManager self, Actor target, float? maxHorizontalAngleDegrees)
    {
        this.self = self;
        this.target = target;
        this.maxHorizontalAngleDegrees = maxHorizontalAngleDegrees;
    }

    public override void Activate()
    {
    }

    public override void Deactivate()
    {
    }

    public override bool Triggers()
    {
        return self.IsFacingTarget(target.transform.position, maxHorizontalAngleDegrees);
    }
}
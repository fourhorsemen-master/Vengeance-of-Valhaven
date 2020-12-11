using UnityEngine;

public class Facing : StateMachineTrigger
{
    private readonly Actor actor;
    private readonly Actor target;
    private readonly float maxAngleDegrees;

    public Facing(Actor actor, Actor target, float maxAngleDegrees)
    {
        this.actor = actor;
        this.target = target;
        this.maxAngleDegrees = maxAngleDegrees;
    }

    public override void Activate()
    {
    }

    public override void Deactivate()
    {
    }

    public override bool Triggers()
    {
        float angleDegrees = Vector3.Angle(actor.transform.forward, target.transform.position - actor.transform.position);

        return angleDegrees <= maxAngleDegrees;
    }
}
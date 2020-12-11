using UnityEngine;

public class IsFacing : StateMachineTrigger
{
    const float MaxAngleDegrees = 10;

    private readonly Actor actor;
    private readonly Actor target;

    public IsFacing(Actor actor, Actor target)
    {
        this.actor = actor;
        this.target = target;
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

        return angleDegrees <= MaxAngleDegrees;
    }
}
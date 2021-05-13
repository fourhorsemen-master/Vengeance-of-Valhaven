using UnityEngine;

public class Facing : StateMachineTrigger
{
    private readonly Actor actor;
    private readonly Actor target;
    private readonly float maxHorizontalAngleDegrees;

    public Facing(Actor actor, Actor target, float maxHorizontalAngleDegrees)
    {
        this.actor = actor;
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
        Vector3 actorHorizontalPosition = actor.transform.forward;
        actorHorizontalPosition.y = 0f;
        Vector3 targetHorizontalPosition = target.transform.position - actor.transform.position;
        targetHorizontalPosition.y = 0f;

        float angleDegrees = Vector3.Angle(actorHorizontalPosition, targetHorizontalPosition);

        return angleDegrees <= maxHorizontalAngleDegrees;
    }
}
using UnityEngine;

[Behaviour("Follow Target At Distance", new string[] { "Follow distance" }, new AIAction[] { AIAction.Advance })]
public class FollowTargetAtDistance : Behaviour
{
    private float _followDistance;

    public override void Initialize()
    {
        _followDistance = Args[0];
    }

    public override void Behave(Actor actor)
    {
        if (!actor.Target)
        {
            return;
        }

        float distanceToTarget = Vector3.Distance(
            actor.transform.position,
            actor.Target.transform.position
        );

        if (distanceToTarget > _followDistance)
        {
            actor.MovementManager.SetDestination(actor.Target.transform.position);
        }
        else
        {
            actor.MovementManager.ClearDestination();
            actor.MovementManager.Watch(actor.Target.transform);
        }
    }
}

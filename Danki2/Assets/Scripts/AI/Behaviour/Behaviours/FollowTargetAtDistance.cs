using UnityEngine;

[Behaviour("Follow Target At Distance", new string[] { "Follow distance" }, new AIAction[] { AIAction.Advance })]
public class FollowTargetAtDistance : Behaviour
{
    private float _followDistance;

    public override void Initialize()
    {
        _followDistance = Args[0];
    }

    public override void Behave(AI ai, Actor actor)
    {
        if (!ai.Target)
        {
            return;
        }

        float distanceToTarget = Vector3.Distance(
            actor.transform.position,
            ai.Target.transform.position
        );

        if (distanceToTarget > _followDistance)
        {
            actor.MoveToward(ai.Target.transform.position);
        } else
        {
            actor.FixNextRotation(ai.Target.transform.position - actor.transform.position);
        }
    }
}

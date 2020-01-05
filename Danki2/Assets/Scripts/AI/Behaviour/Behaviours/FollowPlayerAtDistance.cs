using UnityEngine;

[Behaviour("Follow Player At Distance", new AIAction[] { AIAction.Advance }, new string[] { "followDistance" })]
public class FollowPlayerAtDistance : Behaviour
{
    private float _followDistance;

    public override void Initialise(float[] args)
    {
        _followDistance = args[0];
    }

    public override void Behave(Actor actor)
    {
        // We wouldn't want to be finding the player every frame in the real
        // world, this is just an example.
        GameObject player = GameObject.Find("Player");
        if (Vector3.Distance(actor.transform.position, player.transform.position) > _followDistance)
        {
            actor.MoveToward(player.transform.position);
        }
    }
}

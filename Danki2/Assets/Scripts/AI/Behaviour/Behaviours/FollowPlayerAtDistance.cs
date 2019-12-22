using UnityEngine;

[Behaviour("Follow Player At Distance", AIAction.Advance)]
public class FollowPlayerAtDistance<T> : Behaviour<T> where T : Actor
{
    private readonly float _followDistance;

    public FollowPlayerAtDistance(float[] args) : base(args) {
        _followDistance = args[0];
    }

    public override void Behave(T actor)
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

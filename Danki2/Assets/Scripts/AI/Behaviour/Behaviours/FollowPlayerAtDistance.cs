using UnityEngine;

public class FollowPlayerAtDistance<T> : IBehaviour<T> where T : Actor
{
    private static readonly float _followDistance = 5;

    public void Behave(T actor)
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

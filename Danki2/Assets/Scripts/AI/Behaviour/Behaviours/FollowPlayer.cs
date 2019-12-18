using UnityEngine;

[Behaviour("Follow Player", AIAction.Advance)]
public class FollowPlayer<T> : IBehaviour<T> where T : Actor
{
    public void Behave(T actor)
    {
        // We wouldn't want to be finding the player every frame in the real
        // world, this is just an example.
        GameObject player = GameObject.Find("Player");
        actor.MoveToward(player.transform.position);
    }
}

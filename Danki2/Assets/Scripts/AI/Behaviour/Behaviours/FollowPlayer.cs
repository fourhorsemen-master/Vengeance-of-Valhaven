using System.Collections.Generic;
using UnityEngine;

[Behaviour("Follow Player", AIAction.Advance)]
public class FollowPlayer<T> : Behaviour<T> where T : Actor
{
    public FollowPlayer(List<float> args) : base(args) { } 

    public override void Behave(T actor)
    {
        // We wouldn't want to be finding the player every frame in the real
        // world, this is just an example.
        GameObject player = GameObject.Find("Player");
        actor.MoveToward(player.transform.position);
    }
}

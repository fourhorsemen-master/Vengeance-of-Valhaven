using UnityEngine;

[Behaviour("Follow Player", new string[0], new AIAction[] { AIAction.Advance })]
public class FollowPlayer : Behaviour
{
    public override void Behave(Actor actor)
    {
        // We wouldn't want to be finding the player every frame in the real
        // world, this is just an example.
        GameObject player = GameObject.Find("Player");
        actor.MoveToward(player.transform.position);
    }
}

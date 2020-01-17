using System;
using UnityEngine;

[Behaviour("Attack Player", new string[0], new AIAction[] { AIAction.Attack })]
public class AttackPlayer : Behaviour
{
    public override void Behave(Actor actor)
    {
        Debug.Log("Attacking player...");
    }
}

using System;
using UnityEngine;

[Behaviour("Attack Player", new AIAction[] { AIAction.Attack }, new string[0])]
public class AttackPlayer : Behaviour
{
    public override void Behave(Actor actor)
    {
        Debug.Log("Attacking player...");
    }
}

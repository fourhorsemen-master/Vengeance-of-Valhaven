using System.Collections.Generic;
using UnityEngine;

[Behaviour("Attack Player", AIAction.Attack)]
public class AttackPlayer<T> : Behaviour<T> where T : Actor
{
    public AttackPlayer(List<float> args) : base(args) { }

    public override void Behave(T actor)
    {
        Debug.Log("Attacking player...");
    }
}

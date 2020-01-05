using UnityEngine;

[Behaviour("Attack Player", AIAction.Attack)]
public class AttackPlayer : Behaviour
{
    public override void Behave(Actor actor)
    {
        Debug.Log("Attacking player...");
    }
}

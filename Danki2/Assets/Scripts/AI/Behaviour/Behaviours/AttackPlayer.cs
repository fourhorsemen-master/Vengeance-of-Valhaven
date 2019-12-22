using UnityEngine;

[Behaviour("Attack Player", AIAction.Attack)]
public class AttackPlayer : Behaviour
{
    public AttackPlayer(float[] args) : base(args) { }

    public override void Behave(Actor actor)
    {
        Debug.Log("Attacking player...");
    }
}

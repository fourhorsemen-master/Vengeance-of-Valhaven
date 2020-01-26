using UnityEngine;

[Behaviour("Wolf Attack", new string[0], new AIAction[] { AIAction.Attack })]
public class WolfAttack : Behaviour
{
    public override void Behave(AI ai, Actor actor)
    {
        if (ai.Target)
        {
            Debug.Log("Attacking");
        }
    }
}

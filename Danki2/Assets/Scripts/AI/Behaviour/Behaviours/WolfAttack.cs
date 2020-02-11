using UnityEngine;

[Behaviour("Wolf Attack", new string[] {  }, new AIAction[] { AIAction.Attack })]
public class WolfAttack : Behaviour
{
    public override void Behave(Actor actor)
    {
        Wolf wolf = (Wolf)actor;

        if (!wolf.Target) 
        {
            return;
        }

        float distanceToTarget = Vector3.Distance(
            wolf.transform.position,
            wolf.Target.transform.position
        );

        if (wolf.BiteOffCooldown && distanceToTarget < Bite.Range)
        {
            wolf.Bite();
            return;
        }

        if (
            wolf.PounceOffCooldown
            && distanceToTarget < Pounce.Range
            && distanceToTarget > Pounce.MinRange
        )
        {
            wolf.Pounce();
            return;
        }

    }
}

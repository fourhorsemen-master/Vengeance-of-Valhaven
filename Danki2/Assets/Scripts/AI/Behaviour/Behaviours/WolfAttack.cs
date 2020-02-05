using UnityEngine;

[Behaviour("Wolf Attack", new string[] {  }, new AIAction[] { AIAction.Attack })]
public class WolfAttack : Behaviour
{
    public override void Behave(Actor actor)
    {
        Wolf wolf = (Wolf)actor;

        if (!wolf.BiteOffCooldown
            || !wolf.Target
            || wolf.RemainingChannelDuration > 0
        ) 
        {
            return;
        }

        float distanceToTarget = Vector3.Distance(
            wolf.transform.position,
            wolf.Target.transform.position
        );

        if (distanceToTarget < Bite.Range)
        {
            wolf.Bite();
            return;
        }

        if (wolf.PounceOffCooldown && distanceToTarget < Pounce.Range)
        {
            wolf.Pounce();
            return;
        }

    }
}

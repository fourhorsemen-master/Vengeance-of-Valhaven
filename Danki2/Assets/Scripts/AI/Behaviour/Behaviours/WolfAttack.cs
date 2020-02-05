using UnityEngine;

[Behaviour("Wolf Attack", new string[] { "Bite Cooldown", "Pounce Cooldown" }, new AIAction[] { AIAction.Attack })]
public class WolfAttack : Behaviour
{
    public override void Behave(Actor actor)
    {
        Wolf wolf = (Wolf)actor;

        if (!wolf.biteOffCooldown || !wolf.Target)
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

        if (!wolf.pounceOffCooldown && distanceToTarget < Pounce.Range)
        {
            wolf.Pounce();
            return;
        }

    }
}

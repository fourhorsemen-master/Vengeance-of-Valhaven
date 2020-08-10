using UnityEngine;

[Behaviour("Wolf Attack", new string[] { "Pounce minimum range", "Pounce maximum range" }, new AIAction[] { AIAction.Attack })]
public class WolfAttack : Behaviour
{
    private float _pounceMinRange;
    private float _pounceMaxRange;

    public override void DeserializeArgs()
    {
        _pounceMinRange = Args[0];
        _pounceMaxRange = Args[1];
    }

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
            && distanceToTarget < _pounceMaxRange
            && distanceToTarget > _pounceMinRange
        )
        {
            wolf.Pounce();
        }

    }
}

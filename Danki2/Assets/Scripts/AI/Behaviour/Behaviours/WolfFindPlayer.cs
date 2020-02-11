using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Behaviour("Wolf Find Player", new string[] { "Aggro Range" }, new AIAction[] { AIAction.FindTarget })]
public class WolfFindPlayer : Behaviour
{
    private float _aggroRange;

    public override void Initialize()
    {
        _aggroRange = Args[0];
    }

    public override void Behave(Actor actor)
    {
        Wolf wolf = (Wolf)actor;
        Player target = RoomManager.Instance.Player;

        float distanceToTarget = Vector3.Distance(
            target.transform.position,
            wolf.transform.position
        );

        if (distanceToTarget < _aggroRange || wolf.IsDamaged)
        {
            wolf.Target = target;
            wolf.CallFriends(target);
        }
    }
}

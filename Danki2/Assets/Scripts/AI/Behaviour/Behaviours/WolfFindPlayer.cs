using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Behaviour("Wolf Find Player", new string[] { "Aggro Range", "Howl Range" }, new AIAction[] { AIAction.FindTarget })]
public class WolfFindPlayer : Behaviour
{
    private float _aggroRange;
    private float _howlRange;

    public override void Initialize()
    {
        _aggroRange = Args[0];
        _howlRange = Args[1];
    }

    public override void Behave(AI ai, Actor actor)
    {
        Wolf wolf = (Wolf)actor;
        Player target = RoomManager.Instance.Player;

        float distanceToTarget = Vector3.Distance(
            target.transform.position,
            wolf.transform.position
        );

        if (distanceToTarget < _aggroRange || wolf.IsDamaged)
        {
            ai.Target = target;
            CallFriends(target, wolf);
        }
    }

    private void CallFriends(Player target, Wolf wolf)
    {
        wolf.Howl();

        IEnumerable<Wolf> friends = RoomManager.Instance.ActorCache
            .Select(item =>
            {
                bool isWolf = item.Actor.TryGetComponent(out Wolf friend);
                return isWolf ? friend : null;
            })
            .Where(friend => friend != null);

        foreach (Wolf friend in friends)
        {
            float distance = Vector3.Distance(
                wolf.transform.position,
                friend.transform.position
            );

            if (distance < _howlRange)
            {
                if (friend.TryGetComponent(out AI ai))
                {
                    ai.Target = target;
                }
            }
        }
    }
}

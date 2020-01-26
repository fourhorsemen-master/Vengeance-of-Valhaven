using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Behaviour("Wolf Find Player", new string[] { "Aggro Range", "Howl Range" }, new AIAction[] { AIAction.FindTarget })]
public class WolfFindPlayer : Behaviour
{
    private float _aggroRange;
    private float _howlRange;
    private Player _target = null;

    public override void Initialize()
    {
        _aggroRange = Args[0];
        _howlRange = Args[1];
    }

    public override void Behave(AI ai, Actor actor)
    {
        if (!_target)
        {
            _target = GameObject.FindObjectOfType<Player>();
        }

        float distanceToTarget = Vector3.Distance(
            _target.transform.position,
            actor.transform.position
        );

        if (distanceToTarget < _aggroRange || actor.IsDamaged)
        {
            ai.Target = _target;
            CallFriends(_target, actor);
        }
    }

    private void CallFriends(Player target, Actor actor)
    {
        actor.GetComponent<AudioSource>().Play();

        IEnumerable<Wolf> friends = RoomManager.Instance.ActorCache
            .Select(item =>
            {
                bool isWolf = item.Actor.TryGetComponent(out Wolf wolf);
                return isWolf ? wolf : null;
            })
            .Where(wolf => wolf != null);

        foreach (Wolf wolf in friends)
        {
            float distance = Vector3.Distance(
                actor.transform.position,
                wolf.transform.position
            );

            if (distance < _howlRange)
            {
                if (wolf.TryGetComponent(out AI ai))
                {
                    ai.Target = target;
                }
            }
        }
    }
}

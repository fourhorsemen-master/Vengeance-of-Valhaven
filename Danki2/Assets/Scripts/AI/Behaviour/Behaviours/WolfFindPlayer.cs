using System.Collections;
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
        Wolf wolf = (Wolf)actor;

        if (!_target)
        {
            _target = GameObject.FindObjectOfType<Player>();
        }

        float distanceToTarget = Vector3.Distance(
            _target.transform.position,
            wolf.transform.position
        );

        if (distanceToTarget < _aggroRange || wolf.IsDamaged)
        {
            ai.Target = _target;
            CallFriends(_target, wolf);
        }
    }

    private void CallFriends(Player target, Actor actor)
    {
        Wolf wolf = (Wolf)actor;
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
                wolf.StartCoroutine(AlertFriend(friend, target));
            }
        }
    }

    private IEnumerator AlertFriend(Wolf friend, Player target)
    {
        yield return new WaitForSeconds(1f);
 
        if (friend.TryGetComponent(out AI ai)
            && ai.Target != target)
        {
            ai.Target = target;
            friend.Howl();
        }
    }
}

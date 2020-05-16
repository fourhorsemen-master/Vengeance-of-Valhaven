using UnityEngine;

[Behaviour("Find Player", new string[] { "Aggro Range" }, new AIAction[] { AIAction.FindTarget })]
public class FindPlayer : Behaviour
{
    private float _aggroRange;

    public override void DeserializeArgs()
    {
        _aggroRange = Args[0];
    }

    public override void Behave(Actor actor)
    {
        Debug.Log("Finding player...");

        Player target = RoomManager.Instance.Player;

        float distanceToTarget = Vector3.Distance(
            target.transform.position, 
            actor.transform.position
        );

        if (distanceToTarget < _aggroRange)
        {
            actor.Target = target;
        }
    }
}

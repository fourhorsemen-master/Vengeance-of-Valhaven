using UnityEngine;

[Behaviour("Find Player", new string[] { "Aggro Range" }, new AIAction[] { AIAction.FindTarget })]
public class FindPlayer : Behaviour
{
    private float _aggroRange;
    private Player _target = null;

    public override void Initialize()
    {
        _aggroRange = Args[0];
    }

    public override void Behave(AI ai, Actor actor)
    {
        Debug.Log("Finding player...");

        if (!_target)
        {
            _target = GameObject.FindObjectOfType<Player>();
        }

        float distanceToTarget = Vector3.Distance(
            _target.transform.position, 
            actor.transform.position
        );

        if (distanceToTarget < _aggroRange)
        {
            ai.Target = _target;
        }
    }
}

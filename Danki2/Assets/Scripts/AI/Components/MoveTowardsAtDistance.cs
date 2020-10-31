using UnityEngine;

public class MoveTowardsAtDistance : IAiComponent
{
    private readonly Actor actor;
    private readonly Actor target;
    private readonly float distance;

    public MoveTowardsAtDistance(Actor actor, Actor target, float distance)
    {
        this.actor = actor;
        this.target = target;
        this.distance = distance;
    }

    public void Enter() {}

    public void Exit()
    {
        actor.MovementManager.StopPathfinding();
    }

    public void Update()
    {
        if (Vector3.Distance(actor.transform.position, target.transform.position) > distance)
        {
            actor.MovementManager.StartPathfinding(target.transform.position);
        }
        else
        {
            actor.MovementManager.StopPathfinding();
        }
    }
}

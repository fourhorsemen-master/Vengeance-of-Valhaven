using UnityEngine;

public class MoveTowardsAtDistance : IStateMachineComponent
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
        actor.MovementManager.ClearWatch();
    }

    public void Update()
    {
        if (Vector3.Distance(actor.transform.position, target.transform.position) > distance)
        {
            actor.MovementManager.ClearWatch();
            actor.MovementManager.StartPathfinding(target.transform.position);
        }
        else
        {
            actor.MovementManager.StopPathfinding();
            actor.MovementManager.Watch(target.transform);
        }
    }
}

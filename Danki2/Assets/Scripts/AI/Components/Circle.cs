using UnityEngine;

public class Circle : IStateMachineComponent
{
    private readonly Actor actor;
    private readonly Actor target;

    private CircleDirection circleDirection;

    public Circle(Actor actor, Actor target)
    {
        this.actor = actor;
        this.target = target;
    }

    public void Enter()
    {
        circleDirection = RandomUtils.Choice(CircleDirection.Clockwise, CircleDirection.Anticlockwise);
    }

    public void Exit()
    {
        actor.MovementManager.StopPathfinding();
    }

    public void Update()
    {
        Vector3 position = actor.transform.position;

        Vector3 clockwiseDirection = Vector3.Cross(Vector3.up, target.transform.position - position).normalized;
        Vector3 movementDirection = circleDirection == CircleDirection.Clockwise
            ? clockwiseDirection
            : -clockwiseDirection;

        Vector3 destination = position + movementDirection;

        if (actor.MovementManager.TryStartPathfinding(destination)) return;

        circleDirection = circleDirection == CircleDirection.Clockwise
            ? CircleDirection.Anticlockwise
            : CircleDirection.Clockwise;
    }

    private enum CircleDirection
    {
        Clockwise,
        Anticlockwise
    }
}

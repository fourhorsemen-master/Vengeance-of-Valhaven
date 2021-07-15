using UnityEngine;

public class Circle : IStateMachineComponent
{
    private readonly Enemy enemy;
    private readonly Actor target;

    private CircleDirection circleDirection;

    public Circle(Enemy enemy, Actor target)
    {
        this.enemy = enemy;
        this.target = target;
    }

    public void Enter()
    {
        circleDirection = RandomUtils.Choice(CircleDirection.Clockwise, CircleDirection.Anticlockwise);
    }

    public void Exit()
    {
        enemy.MovementManager.StopPathfinding();
    }

    public void Update()
    {
        Vector3 position = enemy.transform.position;

        Vector3 clockwiseDirection = Vector3.Cross(Vector3.up, target.transform.position - position).normalized;
        Vector3 movementDirection = circleDirection == CircleDirection.Clockwise
            ? clockwiseDirection
            : -clockwiseDirection;

        Vector3 destination = position + movementDirection;

        if (enemy.MovementManager.TryStartPathfinding(destination)) return;

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

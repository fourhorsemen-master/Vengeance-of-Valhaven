using UnityEngine;

public class MoveInRandomDirection : IAiComponent
{
    private readonly Actor actor;

    private Vector3 direction;

    public MoveInRandomDirection(Actor actor)
    {
        this.actor = actor;
    }

    public void Enter()
    {
        Vector2 offset = Random.insideUnitCircle.normalized;
        direction = new Vector3(offset.x, 0f, offset.y);
    }

    public void Exit() {}

    public void Update()
    {
        actor.MovementManager.Move(direction);
    }
}

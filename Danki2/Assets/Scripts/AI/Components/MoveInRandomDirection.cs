using UnityEngine;

public class MoveInRandomDirection : IStateMachineComponent
{
    private readonly Enemy enemy;

    private Vector3 direction;

    public MoveInRandomDirection(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        Vector2 offset = Random.insideUnitCircle.normalized;
        direction = new Vector3(offset.x, 0f, offset.y);
    }

    public void Exit() {}

    public void Update()
    {
        enemy.MovementManager.SetMovementTargetPoint(enemy.transform.position + direction);
    }
}

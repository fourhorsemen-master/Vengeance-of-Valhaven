using UnityEngine;

public class MoveTowardsAtDistance : IStateMachineComponent
{
    private readonly Enemy enemy;
    private readonly Actor target;
    private readonly float distance;
    private readonly float? rotationSpeedMultiplier;

    public MoveTowardsAtDistance(
        Enemy enemy,
        Actor target,
        float distance,
        float? rotationSpeedMultiplier = null
    )
    {
        this.enemy = enemy;
        this.target = target;
        this.distance = distance;
        this.rotationSpeedMultiplier = rotationSpeedMultiplier;
    }

    public void Enter() {}

    public void Exit()
    {
        enemy.MovementManager.StopPathfinding();
    }

    public void Update()
    {
        if (Vector3.Distance(enemy.transform.position, target.transform.position) > distance)
        {
            enemy.MovementManager.SetRotationTargetPoint(target.transform.position, rotationSpeedMultiplier);
        }
        else
        {
            enemy.MovementManager.StopPathfinding();
            enemy.MovementManager.SetRotationTarget(target.transform, rotationSpeedMultiplier);
        }
    }
}

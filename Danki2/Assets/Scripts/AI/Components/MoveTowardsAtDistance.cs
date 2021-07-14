using UnityEngine;

public class MoveTowardsAtDistance : IStateMachineComponent
{
    private readonly Enemy enemy;
    private readonly Actor target;
    private readonly float distance;
    private readonly float? rotationSmoothingOverride;

    public MoveTowardsAtDistance(
        Enemy enemy,
        Actor target,
        float distance,
        float? rotationSmoothingOverride = null
    )
    {
        this.enemy = enemy;
        this.target = target;
        this.distance = distance;
        this.rotationSmoothingOverride = rotationSmoothingOverride;
    }

    public void Enter() {}

    public void Exit()
    {
        enemy.MovementManager.StopPathfinding();
        enemy.MovementManager.ClearWatch();
    }

    public void Update()
    {
        if (Vector3.Distance(enemy.transform.position, target.transform.position) > distance)
        {
            enemy.MovementManager.ClearWatch();
            enemy.MovementManager.StartPathfinding(target.transform.position);
        }
        else
        {
            enemy.MovementManager.StopPathfinding();
            enemy.MovementManager.Watch(target.transform, rotationSmoothingOverride);
        }
    }
}

public class WatchTarget : IStateMachineComponent
{
    private readonly Enemy enemy;
    private readonly Actor target;
    private readonly float? rotationSmoothingOverride;

    public WatchTarget(Enemy enemy, Actor target, float? rotationSmoothingOverride = null)
    {
        this.enemy = enemy;
        this.target = target;
        this.rotationSmoothingOverride = rotationSmoothingOverride;
    }

    public void Enter()
    {
        enemy.MovementManager.Watch(target.transform, rotationSmoothingOverride);
    }

    public void Exit()
    {
        enemy.MovementManager.ClearWatch();
    }

    public void Update() {}
}

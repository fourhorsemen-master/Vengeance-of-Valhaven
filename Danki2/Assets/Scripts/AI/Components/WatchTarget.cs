public class WatchTarget : IStateMachineComponent
{
    private readonly Enemy enemy;
    private readonly Actor target;
    private readonly float? rotationSpeedMultiplier;

    public WatchTarget(Enemy enemy, Actor target, float? rotationSpeedMultiplier = null)
    {
        this.enemy = enemy;
        this.target = target;
        this.rotationSpeedMultiplier = rotationSpeedMultiplier;
    }

    public void Enter()
    {
        enemy.MovementManager.SetRotationTarget(target.transform, rotationSpeedMultiplier);        
    }

    public void Exit()
    {}

    public void Update()
    {}
}

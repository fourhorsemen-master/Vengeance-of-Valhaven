public class WalkTowards : IStateMachineComponent
{
    private readonly Enemy enemy;
    private readonly Actor target;

    public WalkTowards(Enemy enemy, Actor target)
    {
        this.enemy = enemy;
        this.target = target;
    }

    public void Enter()
    {
        enemy.MovementManager.MotionType = MotionType.Walk;
        enemy.MovementManager.SetMovementTarget(target.transform);
        enemy.MovementManager.SetRotationTarget(target.transform, null);
    }

    public void Exit()
    {
        enemy.MovementManager.MotionType = MotionType.Run;
        enemy.MovementManager.SetRotationTarget(null, null);
    }

    public void Update()
    {
        
    }
}
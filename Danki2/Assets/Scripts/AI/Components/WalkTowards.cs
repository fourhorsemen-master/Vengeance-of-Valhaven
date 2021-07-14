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
    }

    public void Exit()
    {
        enemy.MovementManager.MotionType = MotionType.Run;
        enemy.MovementManager.StopPathfinding();
    }

    public void Update()
    {
        enemy.MovementManager.StartPathfinding(target.transform.position);
    }
}
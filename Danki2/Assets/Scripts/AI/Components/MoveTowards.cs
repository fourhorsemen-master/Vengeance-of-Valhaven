public class MoveTowards : IStateMachineComponent
{
    private readonly Enemy enemy;
    private readonly Actor target;

    public MoveTowards(Enemy enemy, Actor target)
    {
        this.enemy = enemy;
        this.target = target;
    }

    public void Enter() {}

    public void Exit()
    {
        enemy.MovementManager.StopPathfinding();
    }

    public void Update()
    {
        enemy.MovementManager.StartPathfinding(target.transform.position);
    }
}

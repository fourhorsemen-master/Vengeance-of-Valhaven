public class MoveAway : IStateMachineComponent
{
    private readonly Enemy enemy;
    private readonly Actor target;

    public MoveAway(Enemy enemy, Actor target)
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
        enemy.MovementManager.StartPathfinding(2 * enemy.transform.position - target.transform.position);
    }
}

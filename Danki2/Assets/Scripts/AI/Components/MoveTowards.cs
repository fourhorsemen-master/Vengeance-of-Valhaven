public class MoveTowards : IStateMachineComponent
{
    private readonly Enemy enemy;
    private readonly Actor target;

    public MoveTowards(Enemy enemy, Actor target)
    {
        this.enemy = enemy;
        this.target = target;
    }

    public void Enter()
    {
        enemy.MovementManager.SetMovementTarget(target.transform);
        enemy.MovementManager.SetRotationTarget(target.transform, null);
    }

    public void Exit()
    {
        enemy.MovementManager.SetMovementTarget(null);
        enemy.MovementManager.SetRotationTarget(null, null);
    }

    public void Update() {}
}

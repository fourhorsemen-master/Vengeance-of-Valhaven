public class TelegraphAttack : IStateMachineComponent
{
    private readonly Enemy enemy;
    private readonly float telegraphTime;

    public TelegraphAttack(Enemy enemy, float telegraphTime)
    {
        this.enemy = enemy;
        this.telegraphTime = telegraphTime;
    }

    public void Enter()
    {
        enemy.MovementManager.StopPathfinding();
        enemy.Telegraph(telegraphTime);
    }

    public void Exit() {}

    public void Update() {}
}

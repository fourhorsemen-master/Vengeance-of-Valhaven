public class TelegraphAttack : IAiComponent
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
        enemy.OnTelegraph.Next(telegraphTime);
    }

    public void Exit() {}

    public void Update() {}
}

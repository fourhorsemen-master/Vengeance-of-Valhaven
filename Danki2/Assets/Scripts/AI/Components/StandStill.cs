public class StandStill : IStateMachineComponent
{
    private readonly Enemy enemy;

    public StandStill(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.MovementManager.StopPathfinding();
    }

    public void Exit() {}

    public void Update() {}
}

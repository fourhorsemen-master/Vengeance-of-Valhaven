public class StandStill : IStateMachineComponent
{
    private readonly Enemy enemy;
    private readonly float? duration;

    public StandStill(Enemy enemy, float? duration)
    {
        this.enemy = enemy;
        this.duration = duration;
    }

    public void Enter()
    {
        enemy.MovementManager.StopPathfinding();
        enemy.MovementManager.Pause(duration);
        enemy.MovementManager.LockRotation(duration);
    }

    public void Exit() 
    {
        enemy.MovementManager.Unpause();
        enemy.MovementManager.UnlockRotation();
    }

    public void Update() {}
}

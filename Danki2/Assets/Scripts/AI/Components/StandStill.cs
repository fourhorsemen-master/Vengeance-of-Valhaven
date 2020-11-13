public class StandStill : IStateMachineComponent
{
    private readonly Actor actor;

    public StandStill(Actor actor)
    {
        this.actor = actor;
    }

    public void Enter()
    {
        actor.MovementManager.StopPathfinding();
    }

    public void Exit() {}

    public void Update() {}
}

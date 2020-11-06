public class MoveAway : IAiComponent
{
    private readonly Actor actor;
    private readonly Actor target;

    public MoveAway(Actor actor, Actor target)
    {
        this.actor = actor;
        this.target = target;
    }

    public void Enter() {}

    public void Exit()
    {
        actor.MovementManager.StopPathfinding();
    }

    public void Update()
    {
        actor.MovementManager.StartPathfinding(2 * actor.transform.position - target.transform.position);
    }
}

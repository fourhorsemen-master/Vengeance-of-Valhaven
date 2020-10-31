public class MoveTowards : IAiComponent
{
    private readonly Actor actor;
    private readonly Actor target;

    public MoveTowards(Actor actor, Actor target)
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
        actor.MovementManager.StartPathfinding(target.transform.position);
    }
}

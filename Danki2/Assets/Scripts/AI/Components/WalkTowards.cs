public class WalkTowards : IStateMachineComponent
{
    private readonly Actor actor;
    private readonly Actor target;

    public WalkTowards(Actor actor, Actor target)
    {
        this.actor = actor;
        this.target = target;
    }

    public void Enter()
    {
        actor.MovementManager.MotionType = MotionType.Walk;
    }

    public void Exit()
    {
        actor.MovementManager.MotionType = MotionType.Run;
        actor.MovementManager.StopPathfinding();
    }

    public void Update()
    {
        actor.MovementManager.StartPathfinding(target.transform.position);
    }
}
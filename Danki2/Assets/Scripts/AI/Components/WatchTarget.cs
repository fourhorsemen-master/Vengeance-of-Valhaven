public class WatchTarget : StateMachineComponent
{
    private readonly Actor actor;
    private readonly Actor target;

    public WatchTarget(Actor actor, Actor target)
    {
        this.actor = actor;
        this.target = target;
    }

    public void Enter()
    {
        actor.MovementManager.Watch(target.transform);
    }

    public void Exit()
    {
        actor.MovementManager.ClearWatch();
    }

    public void Update() {}
}

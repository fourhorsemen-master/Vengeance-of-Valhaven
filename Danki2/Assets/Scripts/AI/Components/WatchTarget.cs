public class WatchTarget : IStateMachineComponent
{
    private readonly Actor actor;
    private readonly Actor target;
    private readonly float? rotationSmoothingOverride;

    public WatchTarget(Actor actor, Actor target, float? rotationSmoothingOverride = null)
    {
        this.actor = actor;
        this.target = target;
        this.rotationSmoothingOverride = rotationSmoothingOverride;
    }

    public void Enter()
    {
        actor.MovementManager.Watch(target.transform, rotationSmoothingOverride);
    }

    public void Exit()
    {
        actor.MovementManager.ClearWatch();
    }

    public void Update() {}
}

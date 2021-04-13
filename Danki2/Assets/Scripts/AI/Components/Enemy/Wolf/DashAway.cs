public class DashAway : IStateMachineComponent
{
    private readonly Wolf wolf;
    private readonly Actor target;

    public DashAway(Wolf wolf, Actor target)
    {
        this.wolf = wolf;
        this.target = target;
    }

    public void Enter() => wolf.DashFromActor(target);
    public void Exit() { }
    public void Update() { }
}

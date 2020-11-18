public class WolfPounce : IStateMachineComponent
{
    private readonly Wolf wolf;
    private readonly Actor target;

    public WolfPounce(Wolf wolf, Actor target)
    {
        this.wolf = wolf;
        this.target = target;
    }

    public void Enter() => wolf.Pounce(target);
    public void Exit() {}
    public void Update() {}
}

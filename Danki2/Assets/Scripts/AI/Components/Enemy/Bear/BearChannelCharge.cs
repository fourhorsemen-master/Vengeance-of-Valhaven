public class BearChannelCharge : IStateMachineComponent
{
    private readonly Bear bear;
    private readonly Actor target;

    public BearChannelCharge(Bear bear, Actor target)
    {
        this.bear = bear;
        this.target = target;
    }

    public void Enter() => bear.Charge(target);
    public void Exit() {}
    public void Update() {}
}

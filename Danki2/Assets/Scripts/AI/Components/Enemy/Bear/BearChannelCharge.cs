public class BearChannelCharge : IStateMachineComponent
{
    private readonly Bear bear;
    private readonly Actor target;

    public BearChannelCharge(Bear bear, Actor target)
    {
        this.bear = bear;
        this.target = target;
    }

    public void Enter()
    {
        UpdateChannelTarget();
        bear.Charge(target);
    }
    public void Exit() { }
    public void Update() => UpdateChannelTarget();

    private void UpdateChannelTarget()
    {
        bear.ChannelService.FloorTargetPosition = target.transform.position;
        bear.ChannelService.OffsetTargetPosition = target.Centre;
    }
}

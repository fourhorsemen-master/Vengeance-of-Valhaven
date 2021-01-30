public class BearChannelCharge : IStateMachineComponent
{
    private readonly Bear bear;

    public BearChannelCharge(Bear bear)
    {
        this.bear = bear;
    }

    public void Enter() => bear.Charge();
    public void Exit() { }
    public void Update()
    {
        bear.ChannelService.FloorTargetPosition = bear.transform.position;
        bear.ChannelService.OffsetTargetPosition = bear.Centre;
    }
}

public class ChannelComplete : StateMachineTrigger
{
    private readonly ChannelService channelService;
    private bool channelHasCompleted;
    private Subscription subscription;

    public ChannelComplete(Actor actor)
    {
        channelService = actor.ChannelService;
    }

    public override void Activate()
    {
        channelHasCompleted = false;
        subscription = channelService.ChannelEndSubject.Subscribe(() => channelHasCompleted = true);
    }
    public override void Deactivate() => subscription.Unsubscribe();
    public override bool Triggers() => channelHasCompleted;
}


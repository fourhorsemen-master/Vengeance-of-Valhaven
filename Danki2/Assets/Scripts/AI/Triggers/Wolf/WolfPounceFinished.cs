using System;

public class WolfPounceFinished : IAiTrigger
{
    private readonly Wolf wolf;

    private bool pounceDone;
    private Subscription pounceSubscription;
    private Guid interruptionId;

    public WolfPounceFinished(Wolf wolf)
    {
        this.wolf = wolf;
    }

    public void Activate()
    {
        pounceDone = false;
        pounceSubscription = wolf.OnPounce.Subscribe(() => pounceDone = true);
        interruptionId = wolf.InterruptionManager.Register(InterruptionType.Hard, () => pounceDone = true);
    }

    public void Deactivate()
    {
        pounceSubscription.Unsubscribe();
        wolf.InterruptionManager.Deregister(interruptionId);
    }

    public bool Triggers()
    {
        return pounceDone;
    }
}

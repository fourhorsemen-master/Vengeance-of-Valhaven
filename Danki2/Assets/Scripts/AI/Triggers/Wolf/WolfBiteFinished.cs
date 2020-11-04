using System;

public class WolfBiteFinished : IAiTrigger
{
    private readonly Wolf wolf;

    private bool biteDone;
    private Subscription biteSubscription;
    private Guid interruptionId;

    public WolfBiteFinished(Wolf wolf)
    {
        this.wolf = wolf;
    }

    public void Activate()
    {
        biteDone = false;
        biteSubscription = wolf.OnBite.Subscribe(() => biteDone = true);
        interruptionId = wolf.InterruptionManager.Register(InterruptionType.Hard, () => biteDone = true);
    }

    public void Deactivate()
    {
        biteSubscription.Unsubscribe();
        wolf.InterruptionManager.Deregister(interruptionId);
    }

    public bool Triggers()
    {
        return biteDone;
    }
}

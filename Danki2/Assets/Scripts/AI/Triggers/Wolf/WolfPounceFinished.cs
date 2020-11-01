public class WolfPounceFinished : IAiTrigger
{
    private readonly Wolf wolf;

    private bool pounceDone;
    private Subscription pounceSubscription;

    public WolfPounceFinished(Wolf wolf)
    {
        this.wolf = wolf;
    }

    public void Activate()
    {
        pounceDone = false;
        pounceSubscription = wolf.OnPounce.Subscribe(() => pounceDone = true);
    }

    public void Deactivate()
    {
        pounceSubscription.Unsubscribe();
    }

    public bool Triggers()
    {
        return pounceDone;
    }
}

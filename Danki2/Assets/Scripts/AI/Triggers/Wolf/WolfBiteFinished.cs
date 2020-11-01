public class WolfBiteFinished : IAiTrigger
{
    private readonly Wolf wolf;

    private bool biteDone;
    private Subscription biteSubscription;

    public WolfBiteFinished(Wolf wolf)
    {
        this.wolf = wolf;
    }

    public void Activate()
    {
        biteDone = false;
        biteSubscription = wolf.OnBite.Subscribe(() => biteDone = true);
    }

    public void Deactivate()
    {
        biteSubscription.Unsubscribe();
    }

    public bool Triggers()
    {
        return biteDone;
    }
}

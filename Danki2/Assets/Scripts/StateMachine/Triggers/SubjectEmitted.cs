public class SubjectEmitted : StateMachineTrigger
{
    private bool emitted;
    private Subscription subscription;

    private readonly Subject subject;

    public SubjectEmitted(Subject subject)
    {
        this.subject = subject;
    }

    public override void Activate()
    {
        emitted = false;
        subscription = subject.Subscribe(() => emitted = true);
    }

    public override void Deactivate()
    {
        subscription.Unsubscribe();
    }

    public override bool Triggers()
    {
        return emitted;
    }
}

public class SubjectTrigger : StateMachineTrigger
{
    private readonly Subject subject;
    private bool subjectEmitted;
    private Subscription subscription;

    public SubjectTrigger(Subject subject)
    {
        this.subject = subject;
        this.subject = subject;
    }

    public override void Activate()
    {
        subjectEmitted = false;

        subscription = subject.Subscribe(() => subjectEmitted = true);
    }

    public override void Deactivate()
    {
        subscription.Unsubscribe();
    }

    public override bool Triggers()
    {
        return subjectEmitted;
    }
}

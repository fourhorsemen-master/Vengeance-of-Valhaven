public class SubjectEmittedTimes : StateMachineTrigger
{
    private int count;
    private Subscription subscription;

    private readonly Subject subject;
    private readonly int requiredCount;

    public SubjectEmittedTimes(Subject subject, int requiredCount)
    {
        this.subject = subject;
        this.requiredCount = requiredCount;
    }

    public override void Activate()
    {
        count = 0;
        subscription = subject.Subscribe(() => count++);
    }

    public override void Deactivate()
    {
        subscription.Unsubscribe();
    }

    public override bool Triggers()
    {
        return count >= requiredCount;
    }
}

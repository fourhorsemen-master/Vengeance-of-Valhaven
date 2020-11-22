using System;

public abstract class SubjectTrigger : StateMachineTrigger
{
    private Subject subject;
    private bool canTrigger;
    private Subscription subscription;

    public SubjectTrigger(Subject subject)
    {
        this.subject = subject;
    }

    public override void Activate()
    {
        canTrigger = false;
        subscription = subject.Subscribe(() => canTrigger = true);
    }

    public override void Deactivate()
    {
        subscription.Unsubscribe();
    }

    public override bool Triggers()
    {
        return canTrigger;
    }
}
public abstract class SubjectTrigger<T> : StateMachineTrigger
{
    private Subject<T> subject;
    private readonly Func<T, bool> triggerPredicate;
    private bool canTrigger;
    private Subscription<T> subscription;

    public SubjectTrigger(Subject<T> subject, Func<T, bool> triggerPredicate)
    {
        this.subject = subject;
        this.triggerPredicate = triggerPredicate;
    }

    public override void Activate()
    {
        canTrigger = false;
        subscription = subject.Subscribe(x => canTrigger = canTrigger || triggerPredicate(x));
    }

    public override void Deactivate()
    {
        subscription.Unsubscribe();
    }

    public override bool Triggers()
    {
        return canTrigger;
    }
}
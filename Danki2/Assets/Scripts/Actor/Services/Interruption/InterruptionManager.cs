using System;

public class InterruptionManager
{
    private readonly Registry<Interruptable> interruptables;

    private readonly Actor actor;

    public InterruptionManager(Actor actor, Subject startSubject, Subject updateSubject)
    {
        this.actor = actor;

        interruptables = new Registry<Interruptable>(updateSubject);

        startSubject.Subscribe(Setup);
    }

    private void Setup()
    {
        actor.DeathSubject.Subscribe(OnDeath);
        actor.MovementManager.MoveLockSubject.Subscribe(HardInterrupt);
    }

    public Guid Register(InterruptionType type, Action onInterrupt, params InterruptableFeature[] features)
    {
        Interruptable interruptable = new Interruptable(type, onInterrupt, features);
        Guid id = interruptables.AddIndefinite(interruptable);

        return id;
    }

    public void Deregister(Guid id) => interruptables.Remove(id);

    public void Interrupt(InterruptionType interruptionType)
    {
        if (actor.Dead) return;

        switch (interruptionType)
        {
            case InterruptionType.Soft:
                SoftInterrupt();
                break;
            case InterruptionType.Hard:
                HardInterrupt();
                break;
        }
    }

    private void SoftInterrupt()
    {
        InterruptWhere(i => i.Type == InterruptionType.Soft);
    }

    private void HardInterrupt()
    {
        InterruptWhere(i =>
            i.Type == InterruptionType.Soft
            || i.Type == InterruptionType.Hard
        );
    }

    private void OnDeath()
    {
        InterruptWhere(i => i.InterruptOnDeath);
    }

    private void InterruptWhere(Predicate<Interruptable> predicate)
    {
        interruptables.ForEach(i => i.OnInterrupt(), predicate);

        interruptables.RemoveWhere(i => predicate(i) && !i.Repeat);
    }
}

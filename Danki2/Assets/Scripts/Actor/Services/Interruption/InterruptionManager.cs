using System;

public class InterruptionManager
{
    private readonly Registry<Interruptible> interruptibles;

    private readonly Actor actor;

    public InterruptionManager(Actor actor, Subject startSubject, Subject updateSubject)
    {
        this.actor = actor;

        interruptibles = new Registry<Interruptible>(updateSubject);

        startSubject.Subscribe(Setup);
    }

    private void Setup()
    {
        actor.DeathSubject.Subscribe(OnDeath);
        actor.MovementManager.MoveLockSubject.Subscribe(HardInterrupt);
    }

    public Guid Register(InterruptionType type, Action onInterrupt, params InterruptibleFeature[] features)
    {
        Interruptible interruptible = new Interruptible(type, onInterrupt, features);
        Guid id = interruptibles.AddIndefinite(interruptible);

        return id;
    }

    public void Deregister(Guid id) => interruptibles.Remove(id);

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

    private void InterruptWhere(Predicate<Interruptible> predicate)
    {
        interruptibles.ForEach(i => i.OnInterrupt(), predicate);

        interruptibles.RemoveWhere(i => predicate(i) && !i.Repeat);
    }
}

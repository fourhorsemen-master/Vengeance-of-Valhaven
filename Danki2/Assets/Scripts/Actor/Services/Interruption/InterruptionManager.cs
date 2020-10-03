using System;
using System.Collections.Generic;

public class InterruptionManager
{
    private readonly List<Interruptable> interruptables = new List<Interruptable>();

    private readonly Actor actor;

    public InterruptionManager(Actor actor, Subject startSubject)
    {
        this.actor = actor;
        startSubject.Subscribe(Setup);
    }

    private void Setup()
    {
        actor.DeathSubject.Subscribe(OnDeath);
        actor.MovementManager.MoveLockSubject.Subscribe(HardInterrupt);
    }

    public Guid Register(InterruptionType type, Action onInterrupt, bool repeat, bool interruptOnDeath)
    {
        Interruptable interruptable = new Interruptable(type, onInterrupt, repeat, interruptOnDeath);
        interruptables.Add(interruptable);

        return interruptable.Id;
    }

    public void Deregister(Guid id)
    {
        interruptables.RemoveAll(i => i.Id == id);
    }

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
        InterruptWhere(i => i.Threshold == InterruptionType.Soft);
    }

    private void HardInterrupt()
    {
        InterruptWhere(i =>
            i.Threshold == InterruptionType.Soft
            || i.Threshold == InterruptionType.Hard
        );
    }

    private void OnDeath()
    {
        InterruptWhere(i => i.InterruptOnDeath);
    }

    private void InterruptWhere(Predicate<Interruptable> predicate)
    {
        var toInterrupt = interruptables.Where(i => predicate(i));

        toInterrupt.ForEach(i =>
        {
            i.OnInterrupt();
            if (!i.Repeat) interruptables.Remove(i);
        });
    }
}

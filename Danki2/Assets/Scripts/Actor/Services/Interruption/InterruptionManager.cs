using System;
using System.Collections.Generic;
using System.Linq;

public class InterruptionManager
{
    private readonly Dictionary<Guid, Interruptable> interruptables = new Dictionary<Guid, Interruptable>();

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

    public Guid Register(InterruptionType type, Action onInterrupt, params InterruptableFeature[] features)
    {
        Guid id = Guid.NewGuid();
        Interruptable interruptable = new Interruptable(type, onInterrupt, features);
        interruptables.Add(id, interruptable);

        return id;
    }

    public void Deregister(Guid id)
    {
        if (interruptables.ContainsKey(id))
        {
            interruptables.Remove(id);
        }
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
        interruptables.ToList()
            .ForEach(kvp =>
            {
                Interruptable interruptable = kvp.Value;

                if (predicate(interruptable))
                {
                    interruptable.OnInterrupt();
                    if (!interruptable.Repeat) interruptables.Remove(kvp.Key);
                }
            });
    }
}

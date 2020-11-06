using System;

public class Interrupted : IAiTrigger
{
    private readonly Actor actor;
    private readonly InterruptionType interruptionType;

    private bool interrupted;
    private Guid interruptionId;

    public Interrupted(Actor actor, InterruptionType interruptionType)
    {
        this.actor = actor;
        this.interruptionType = interruptionType;
    }

    public void Activate()
    {
        interrupted = false;
        interruptionId = actor.InterruptionManager.Register(interruptionType, () => interrupted = true);
    }

    public void Deactivate()
    {
        actor.InterruptionManager.Deregister(interruptionId);
    }

    public bool Triggers()
    {
        return interrupted;
    }
}

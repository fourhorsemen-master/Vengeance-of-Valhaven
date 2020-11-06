using System;

public class Interrupted : AiTrigger
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

    public override void Activate()
    {
        interrupted = false;
        interruptionId = actor.InterruptionManager.Register(interruptionType, () => interrupted = true);
    }

    public override void Deactivate()
    {
        actor.InterruptionManager.Deregister(interruptionId);
    }

    public override bool Triggers()
    {
        return interrupted;
    }
}

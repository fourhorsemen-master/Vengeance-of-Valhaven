using System;

public class Interruptable
{
    public Guid Id { get; }
    public InterruptionType Threshold { get; }
    public bool Repeat { get; }
    public bool InterruptOnDeath { get; }
    public Action OnInterrupt { get; }

    public Interruptable(InterruptionType type, Action onInterrupt, bool repeat, bool interruptOnDeath)
    {
        Id = Guid.NewGuid();
        Threshold = type;
        Repeat = repeat;
        InterruptOnDeath = interruptOnDeath;
        OnInterrupt = onInterrupt;
    }

}

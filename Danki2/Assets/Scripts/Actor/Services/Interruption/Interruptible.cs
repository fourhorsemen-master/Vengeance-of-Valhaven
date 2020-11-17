using System;
using System.Linq;

public class Interruptible
{
    public InterruptionType Type { get; }
    public Action OnInterrupt { get; }
    public bool Repeat { get; }
    public bool InterruptOnDeath { get; }

    public Interruptible(InterruptionType type, Action onInterrupt, InterruptibleFeature[] features)
    {
        Type = type;
        OnInterrupt = onInterrupt;
        Repeat = features.Contains(InterruptibleFeature.Repeat);
        InterruptOnDeath = features.Contains(InterruptibleFeature.InterruptOnDeath);
    }

}

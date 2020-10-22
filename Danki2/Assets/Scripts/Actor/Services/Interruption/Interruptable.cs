using System;
using System.Linq;

public class Interruptable
{
    public InterruptionType Type { get; }
    public Action OnInterrupt { get; }
    public bool Repeat { get; }
    public bool InterruptOnDeath { get; }

    public Interruptable(InterruptionType type, Action onInterrupt, InterruptableFeature[] features)
    {
        Type = type;
        OnInterrupt = onInterrupt;
        Repeat = features.Contains(InterruptableFeature.Repeat);
        InterruptOnDeath = features.Contains(InterruptableFeature.InterruptOnDeath);
    }

}

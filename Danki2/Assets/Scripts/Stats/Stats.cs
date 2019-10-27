using System;

public enum Stat
{
    Health,
    Speed,
    Strength
}

[Serializable]
public class Stats : SerializableEnumDictionary<Stat, int>
{
    public Stats(int defaultValue) : base(defaultValue) { }
}


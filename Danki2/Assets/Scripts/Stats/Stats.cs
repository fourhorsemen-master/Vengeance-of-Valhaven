using System;

public enum Stat
{
    Health,
    Speed,
    Strength
}

[Serializable]
public class Stats : SerializableDictionary<Stat, int> {}

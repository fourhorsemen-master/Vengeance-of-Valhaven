﻿using System;

public enum Stat
{
    MaxHealth,
    Speed,
    Strength
}

[Serializable]
public class Stats : SerializableEnumDictionary<Stat, int>
{
    public Stats(int defaultValue) : base(defaultValue)
    {
    }

    public Stats(SerializableEnumDictionary<Stat, int> dictionary) : base(dictionary)
    {
    }
}


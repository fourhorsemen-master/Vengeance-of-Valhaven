using System.Collections.Generic;
using UnityEngine;

public static class RandomUtils
{
    private const int MinSeedValue = 0;
    private const int MaxSeedValue = int.MaxValue - 1;

    public static T Choice<T>(params T[] choices)
    {
        return choices[Random.Range(0, choices.Length)];
    }

    public static T Choice<T>(List<T> choices)
    {
        return choices[Random.Range(0, choices.Count)];
    }

    public static int Seed()
    {
        return Random.Range(MinSeedValue, MaxSeedValue + 1);
    }
}

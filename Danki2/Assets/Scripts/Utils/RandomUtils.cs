﻿using UnityEngine;

public static class RandomUtils
{
    public static T Choice<T>(params T[] choices)
    {
        return choices[Random.Range(0, choices.Length)];
    }
}
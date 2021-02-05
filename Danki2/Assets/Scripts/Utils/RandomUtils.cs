using UnityEngine;

public static class RandomUtils
{
    public static T Choice<T>(params T[] choices)
    {
        float rand = Random.value;
        if (rand == 1f) rand = 0f;

        return choices[(int)(rand * choices.Length)];
    }
}

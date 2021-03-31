using System;

public static class Utils
{
    public static void Repeat(int times, Action action)
    {
        for (int _ = 0; _ < times; _++) action();
    }
}

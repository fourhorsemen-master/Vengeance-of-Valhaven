using System;

public static class Repeat
{
    public static void Times(int count, Action action)
    {
        for (int i = 0; i < count; i++) action();
    }
}

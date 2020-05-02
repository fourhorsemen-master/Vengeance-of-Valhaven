using System.Collections.Generic;

public static class ListUtils
{
    public static List<T> Singleton<T>(T item)
    {
        return new List<T> { item };
    }
}

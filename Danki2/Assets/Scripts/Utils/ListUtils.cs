using System.Collections.Generic;

public static class ListUtils
{
    /// <summary>
    /// Helper method that returns a list containing the one given item.
    /// </summary>
    /// <param name="item"> The item to be in the list </param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> Singleton<T>(T item)
    {
        return new List<T> { item };
    }
}

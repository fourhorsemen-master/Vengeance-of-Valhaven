using System;
using System.Collections.Generic;
using System.Linq;

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

    public static List<T> Where<T>(this List<T> list, Func<T, bool> filter)
    {
        return (list as IEnumerable<T>).Where(item => filter(item)).ToList();
    }
}

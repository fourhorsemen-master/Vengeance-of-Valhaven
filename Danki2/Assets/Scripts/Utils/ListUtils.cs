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

    public static bool IsDistinct<T>(this List<T> list)
    {
        return list.GroupBy(s => s).Count() == list.Count;
    }
    
    /// <summary>
    /// Returns true iff the given list contains elements with distinct IDs.
    /// </summary>
    public static bool IsDistinctById<T>(this List<T> list) where T : IIdentifiable
    {
        return list.GroupBy(s => s.Id).Count() == list.Count;
    }

    /// <summary>
    /// Sorts the list in ascending order based on the elements' IDs.
    /// </summary>
    public static void SortById<T>(this List<T> list) where T : IIdentifiable
    {
        list.Sort((a, b) => a.Id == b.Id ? 0 : a.Id < b.Id ? -1 : 1);
    }

    /// <summary>
    /// Helper method to allow adding items to lists with param syntax.
    /// </summary>
    public static void Add<T>(this List<T> list, params T[] items)
    {
        list.AddRange(items);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumUtils
{
    private static readonly Dictionary<Type, Array> arrayCache = new Dictionary<Type, Array>();

    public static int GetLength<TEnum>() where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum)).Length;
    }

    public static void ForEach<TEnum>(Action<TEnum> action) where TEnum : Enum
    {
        Type type = typeof(TEnum);

        if (!arrayCache.TryGetValue(type, out Array array))
        {
            array = Enum.GetValues(type);
            arrayCache[type] = array;
        }

        foreach (TEnum value in array)
        {
            action(value);
        }
    }

    public static TEnum FromString<TEnum>(string raw) where TEnum : Enum
    {
        return (TEnum) Enum.Parse(typeof(TEnum), raw);
    }

    public static List<TEnum> ToList<TEnum>() where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
    }
}

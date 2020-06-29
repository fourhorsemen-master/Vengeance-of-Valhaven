using System;

public static class EnumUtils
{
    public static void ForEach<TEnum>(Action<TEnum> action) where TEnum : Enum
    {
        foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
        {
            action(value);
        }
    }
}

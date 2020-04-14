using System;
using System.Collections.Generic;

public class EnumDictionary<TEnumKey, TValue> : Dictionary<TEnumKey, TValue> where TEnumKey : Enum
{
    public EnumDictionary(TValue defaultValue)
    {
        foreach (TEnumKey key in Enum.GetValues(typeof(TEnumKey)))
        {
            Add(key, defaultValue);
        }
    }
}
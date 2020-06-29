using System;
using System.Collections.Generic;

public class EnumDictionary<TEnumKey, TValue> : Dictionary<TEnumKey, TValue> where TEnumKey : Enum
{
    public EnumDictionary(Func<TValue> defaultValueProvider)
    {
        EnumUtils.ForEach<TEnumKey>(key => Add(key, defaultValueProvider()));
    }
    public EnumDictionary(TValue defaultValue)
    {
        EnumUtils.ForEach<TEnumKey>(key => Add(key, defaultValue));
    }
    
    public EnumDictionary(EnumDictionary<TEnumKey, TValue> enumDictionary)
        : base(enumDictionary) {}
}
﻿using System;
using System.Collections.Generic;

public static class BindablePropertyLookup
{
    private static readonly Dictionary<BindableProperty, string> bindablePropertyToString =
        new Dictionary<BindableProperty, string>
        {
            {BindableProperty.PrimaryDamage, "PRIMARY_DAMAGE"},
            {BindableProperty.SecondaryDamage, "SECONDARY_DAMAGE"},
            {BindableProperty.Heal, "HEAL"},
            {BindableProperty.Shield, "SHIELD"}
        };
    private static readonly Dictionary<string, BindableProperty> stringToBindableProperty = new Dictionary<string, BindableProperty>();

    static BindablePropertyLookup()
    {
        EnumUtils.ForEach<BindableProperty>(bindableProperty =>
        {
            stringToBindableProperty.Add(bindablePropertyToString[bindableProperty], bindableProperty);
        });
    }

    public static bool IsValidBindableProperty(string value)
    {
        return stringToBindableProperty.ContainsKey(value);
    }

    public static BindableProperty FromString(string value)
    {
        return stringToBindableProperty[value];
    }
}

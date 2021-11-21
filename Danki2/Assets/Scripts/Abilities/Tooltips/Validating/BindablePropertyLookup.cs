using System.Collections.Generic;

public static class BindablePropertyLookup
{
    private static readonly Dictionary<BindableProperty, string> bindablePropertyToString =
        new Dictionary<BindableProperty, string>
        {
            {BindableProperty.Heal, "HEAL"},
            {BindableProperty.Stun, "STUN"},
            {BindableProperty.Slow, "SLOW"},
            {BindableProperty.Block, "BLOCK"},
            {BindableProperty.Bleed, "BLEED"},
            {BindableProperty.Poison, "POISON"},
            {BindableProperty.Vulnerable, "VULNERABLE"},
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

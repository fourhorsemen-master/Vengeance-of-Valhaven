using System.Collections.Generic;

public static class BindablePropertyLookup
{
    private static readonly Dictionary<BindableProperty, string> bindablePropertyToString =
        new Dictionary<BindableProperty, string>
        {
            {BindableProperty.PrimaryDamage, "PRIMARY_DAMAGE"},
            {BindableProperty.SecondaryDamage, "SECONDARY_DAMAGE"},
            {BindableProperty.Heal, "HEAL"},
            {BindableProperty.Shield, "SHIELD"},
            {BindableProperty.Stun, "STUN"},
            {BindableProperty.PassiveSlow, "PASSIVE_SLOW"},
            {BindableProperty.Block, "BLOCK"},
            {BindableProperty.StackingSlow, "STACKING_SLOW"},
            {BindableProperty.Bleed, "BLEED"},
            {BindableProperty.Poison, "POISON"},
            {BindableProperty.Vulnerable, "VULNERABLE"},
        };
    private static readonly Dictionary<string, BindableProperty> stringToBindableProperty = new Dictionary<string, BindableProperty>();

    private static readonly Dictionary<BindableProperty, bool> argumentRequirementLookup =
        new Dictionary<BindableProperty, bool>
        {
            {BindableProperty.PrimaryDamage, false},
            {BindableProperty.SecondaryDamage, false},
            {BindableProperty.Heal, false},
            {BindableProperty.Shield, false},
            {BindableProperty.Stun, false},
            {BindableProperty.PassiveSlow, false},
            {BindableProperty.Block, false},
            {BindableProperty.StackingSlow, true},
            {BindableProperty.Bleed, true},
            {BindableProperty.Poison, true},
            {BindableProperty.Vulnerable, true},
        };

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

    public static bool RequiresArgument(BindableProperty bindableProperty)
    {
        return argumentRequirementLookup[bindableProperty];
    }
}

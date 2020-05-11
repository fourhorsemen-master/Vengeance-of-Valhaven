using System;

[AttributeUsage(AttributeTargets.Class)]
public class AbilityAttribute : Attribute
{
    public AbilityReference AbilityReference { get; }

    public AbilityAttribute(AbilityReference abilityReference)
    {
        AbilityReference = abilityReference;
    }
}

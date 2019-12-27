using System;

[AttributeUsage(AttributeTargets.Class)]
public class PlannerAttribute : Attribute
{
    public string DisplayValue { get; }

    public PlannerAttribute(string displayValue)
    {
        DisplayValue = displayValue;
    }
}

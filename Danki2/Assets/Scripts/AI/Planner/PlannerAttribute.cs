using System;

[AttributeUsage(AttributeTargets.Class)]
public class PlannerAttribute : Attribute
{
    public string DisplayValue { get; }
    public string[] Args { get; }

    public PlannerAttribute(string displayValue, string[] args)
    {
        DisplayValue = displayValue;
        Args = args;
    }
}

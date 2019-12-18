using System;

[AttributeUsage(AttributeTargets.Class)]
public class BehaviourAttribute : Attribute
{
    public string DisplayValue { get; }
    public AIAction Action { get; }

    public BehaviourAttribute(string displayValue, AIAction action)
    {
        DisplayValue = displayValue;
        Action = action;
    }
}

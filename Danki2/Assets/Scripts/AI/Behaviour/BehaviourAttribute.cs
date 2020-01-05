using System;

[AttributeUsage(AttributeTargets.Class)]
public class BehaviourAttribute : Attribute
{
    public string DisplayValue { get; }
    public AIAction[] Actions { get; }
    public string[] Args { get; }

    public BehaviourAttribute(string displayValue, AIAction[] actions, string[] args)
    {
        DisplayValue = displayValue;
        Actions = actions;
        Args = args;
    }
}

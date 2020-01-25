using System;

[AttributeUsage(AttributeTargets.Class)]
public class BehaviourAttribute : AIAttribute
{
    public AIAction[] Actions { get; }

    public BehaviourAttribute(string displayValue, string[] argLabels, AIAction[] actions) : base(displayValue, argLabels)
    {
        Actions = actions;
    }
}

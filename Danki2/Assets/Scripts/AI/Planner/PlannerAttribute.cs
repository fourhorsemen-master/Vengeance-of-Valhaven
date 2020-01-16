using System;

[AttributeUsage(AttributeTargets.Class)]
public class PlannerAttribute : AIAttribute
{
    public PlannerAttribute(string displayValue, string[] argLabels) : base(displayValue, argLabels)
    {
    }
}

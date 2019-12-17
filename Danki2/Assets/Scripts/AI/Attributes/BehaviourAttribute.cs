using System;

[AttributeUsage(AttributeTargets.Class)]
public class BehaviourAttribute : Attribute
{
    public string SomeValue { get; set; }
    public Type Type { get; set; }
}

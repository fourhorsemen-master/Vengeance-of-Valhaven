using System;

public abstract class AIAttribute : Attribute
{
    public string DisplayValue { get; }
    public string[] ArgLabels { get; }

    public AIAttribute(string displayValue, string[] argLabels)
    {
        DisplayValue = displayValue;
        ArgLabels = argLabels;
    }
}

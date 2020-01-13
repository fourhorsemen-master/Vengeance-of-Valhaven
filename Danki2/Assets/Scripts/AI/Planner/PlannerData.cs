using System;

public class PlannerData
{
    public string DisplayValue { get; }
    public string[] Args { get; }
    public Type Planner { get; }

    public PlannerData(AttributeData<PlannerAttribute> attributeData)
    {
        DisplayValue = attributeData.Attribute.DisplayValue;
        Args = attributeData.Attribute.Args;
        Planner = attributeData.Type;
    }
}

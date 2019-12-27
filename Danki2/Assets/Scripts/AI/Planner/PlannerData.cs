using System;

public class PlannerData
{
    public string DisplayValue { get; }
    public Type Planner { get; }

    public PlannerData(AttributeData<PlannerAttribute> attributeData)
    {
        DisplayValue = attributeData.Attribute.DisplayValue;
        Planner = attributeData.Type;
    }
}

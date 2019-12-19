using System;

public class BehaviourData
{
    public string DisplayValue { get; }
    public AIAction Action { get; }
    public Type Type { get; }

    public BehaviourData(AttributeData<BehaviourAttribute> attributeData)
    {
        DisplayValue = attributeData.Attribute.DisplayValue;
        Action = attributeData.Attribute.Action;
        Type = attributeData.Type;
    }
}

using System;

public class BehaviourData
{
    public string DisplayValue { get; }
    public AIAction Action { get; }
    public Type Behaviour { get; }

    public BehaviourData(AttributeData<BehaviourAttribute> attributeData)
    {
        DisplayValue = attributeData.Attribute.DisplayValue;
        Action = attributeData.Attribute.Action;
        Behaviour = attributeData.Type;
    }
}

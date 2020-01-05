using System;

public class BehaviourData
{
    public string DisplayValue { get; }
    public AIAction[] Actions { get; }
    public Type Behaviour { get; }

    public BehaviourData(AttributeData<BehaviourAttribute> attributeData)
    {
        DisplayValue = attributeData.Attribute.DisplayValue;
        Actions = attributeData.Attribute.Actions;
        Behaviour = attributeData.Type;
    }
}

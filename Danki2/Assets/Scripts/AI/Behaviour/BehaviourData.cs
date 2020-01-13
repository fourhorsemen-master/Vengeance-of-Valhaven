using System;

public class BehaviourData
{
    public string DisplayValue { get; }
    public AIAction[] Actions { get; }
    public string[] Args { get; }
    public Type Behaviour { get; }

    public BehaviourData(AttributeData<BehaviourAttribute> attributeData)
    {
        DisplayValue = attributeData.Attribute.DisplayValue;
        Actions = attributeData.Attribute.Actions;
        Args = attributeData.Attribute.Args;
        Behaviour = attributeData.Type;
    }
}

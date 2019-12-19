using System;

public class AttributeData<TAttribute> where TAttribute : Attribute
{
    public TAttribute Attribute { get; }
    public Type Type { get; }

    public AttributeData(TAttribute attribute, Type type)
    {
        Attribute = attribute;
        Type = type;
    }
}

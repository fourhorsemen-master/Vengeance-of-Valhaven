using System;

[Serializable]
public class CollisionTemplateDictionary : SerializableEnumDictionary<CollisionTemplateShape, CollisionTemplate>
{
    public CollisionTemplateDictionary(CollisionTemplate defaultValue) : base(defaultValue)
    {
    }

    public CollisionTemplateDictionary(Func<CollisionTemplate> defaultValueProvider) : base(defaultValueProvider)
    {
    }
}

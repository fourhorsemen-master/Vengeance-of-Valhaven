using System;
using Object = UnityEngine.Object;

[Serializable]
public class CollisionTemplateDictionary : SerializableEnumDictionary<CollisionTemplate, Object>
{
    public CollisionTemplateDictionary(Object defaultValue) : base(defaultValue)
    {
    }

    public CollisionTemplateDictionary(SerializableEnumDictionary<CollisionTemplate, Object> dictionary) : base(dictionary)
    {
    }
}

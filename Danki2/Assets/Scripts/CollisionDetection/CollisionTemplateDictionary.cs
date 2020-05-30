using System;
using UnityEngine;

[Serializable]
public class CollisionTemplateDictionary : SerializableEnumDictionary<CollisionTemplate, MeshCollider>
{
    public CollisionTemplateDictionary(MeshCollider defaultValue) : base(defaultValue)
    {
    }

    public CollisionTemplateDictionary(Func<MeshCollider> defaultValueProvider) : base(defaultValueProvider)
    {
    }
}

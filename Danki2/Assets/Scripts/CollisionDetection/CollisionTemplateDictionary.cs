using System;
using UnityEngine;

[Serializable]
public class CollisionTemplateDictionary : SerializableEnumDictionary<CollisionTemplate, MeshCollider>
{
    public CollisionTemplateDictionary(MeshCollider defaultValue) : base(defaultValue)
    {
    }

    public CollisionTemplateDictionary(SerializableEnumDictionary<CollisionTemplate, MeshCollider> dictionary) : base(dictionary)
    {
    }
}

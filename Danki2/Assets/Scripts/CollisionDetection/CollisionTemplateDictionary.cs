using System;
using UnityEngine;

[Serializable]
public class CollisionTemplateDictionary : SerializableEnumDictionary<CollisionTemplate, Collider>
{
    public CollisionTemplateDictionary(Collider defaultValue) : base(defaultValue)
    {
    }

    public CollisionTemplateDictionary(SerializableEnumDictionary<CollisionTemplate, Collider> dictionary) : base(dictionary)
    {
    }
}

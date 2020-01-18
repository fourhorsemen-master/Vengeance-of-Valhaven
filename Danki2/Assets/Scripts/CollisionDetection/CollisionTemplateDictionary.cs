using UnityEngine;

public class CollisionTemplateDictionary : SerializableEnumDictionary<CollisionTemplate, GameObject>
{
    public CollisionTemplateDictionary(GameObject defaultValue) : base(defaultValue)
    {
    }

    public CollisionTemplateDictionary(SerializableEnumDictionary<CollisionTemplate, GameObject> dictionary) : base(dictionary)
    {
    }
}

using UnityEngine;

public class CollisionTemplateManager : Singleton<CollisionTemplateManager>
{
    public CollisionTemplateDictionary prefabLookup = new CollisionTemplateDictionary((CollisionTemplate)null);

    public CollisionTemplate Create(Actor owner, CollisionTemplateShape shape, Vector3 scale, Vector3 position, Quaternion rotation)
    {
        return CollisionTemplate.Create(prefabLookup[shape], owner, scale, position, rotation);
    }
}

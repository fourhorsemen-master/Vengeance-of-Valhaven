using UnityEngine;

public class CollisionTemplateManager : Singleton<CollisionTemplateManager>
{
    public CollisionTemplateDictionary prefabLookup = new CollisionTemplateDictionary((CollisionTemplate)null);

    public CollisionTemplate Create(CollisionTemplateShape shape, Vector3 scale, Vector3 position, Quaternion rotation)
    {
        CollisionTemplate template = Instantiate(prefabLookup[shape], position, rotation);

        template.SetScale(scale);

        return template;
    }
}

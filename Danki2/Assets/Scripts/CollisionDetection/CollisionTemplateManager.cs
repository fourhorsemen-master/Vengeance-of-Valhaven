using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class CollisionTemplateManager : Singleton<CollisionTemplateManager>
{
    public CollisionTemplateDictionary prefabLookup = new CollisionTemplateDictionary((Object)null);

    private CollisionTemplateDictionary instanceLookup = new CollisionTemplateDictionary((Object)null);

    protected override void Awake()
    {
        base.Awake();

        foreach (CollisionTemplate template in Enum.GetValues(typeof(CollisionTemplate)))
        {
            Object prefab = prefabLookup[template];
            if (prefab == null)
            {
                Debug.LogError($"No prefab found for template: {template.ToString()}");
                continue;
            }
            instanceLookup[template] = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }
    }
}
